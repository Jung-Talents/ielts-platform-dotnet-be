using Google.Apis.Auth;
using IeltsPlatform.Domain.Common;
using IeltsPlatform.Domain.DTOs.Auth;
using IeltsPlatform.Domain.Entities;
using IeltsPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IeltsPlatform.Infrastructure.Services;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterDto dto);
    Task<bool> VerifyOtpAsync(VerifyOtpDto dto);
    Task<bool> CancelRegisterAsync(string email);
    Task<LoginResponseDto> LoginAsync(LoginDto dto, string? userAgent, string? ipAddress);
    Task<LoginResponseDto> GoogleLoginAsync(string googleToken);
    Task LogoutAsync(Guid userId, string refreshToken);
    Task<LoginResponseDto> RefreshTokenAsync(string refreshToken, string? ipAddress);
}

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtService;
    private readonly IOtpService _otpService;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;

    public AuthService(
        ApplicationDbContext context,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtService,
        IOtpService otpService,
        IEmailService emailService,
        IConfiguration configuration)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
        _otpService = otpService;
        _emailService = emailService;
        _configuration = configuration;
    }

    public async Task<bool> RegisterAsync(RegisterDto dto)
    {
        // Check if user already exists
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Email is already registered");
        }

        // Delete any previous temp user with this email
        var existingTempUser = await _context.TempUsers.FirstOrDefaultAsync(t => t.Email == dto.Email);
        if (existingTempUser != null)
        {
            _context.TempUsers.Remove(existingTempUser);
        }

        // Create new temp user
        var hashedPassword = _passwordHasher.HashPassword(dto.Password);
        var verificationCode = _otpService.GenerateOtp();
        var expiresAt = DateTime.UtcNow.AddMinutes(5);

        var tempUser = new TempUser
        {
            Email = dto.Email,
            Username = dto.FullName,
            Password = hashedPassword,
            VerificationCode = verificationCode,
            ExpiresAt = expiresAt
        };

        _context.TempUsers.Add(tempUser);
        await _context.SaveChangesAsync();

        // Send verification email (fire and forget)
        _ = Task.Run(async () =>
        {
            try
            {
                await _emailService.SendVerificationCodeAsync(dto.Email, verificationCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send verification email: {ex.Message}");
            }
        });

        return true;
    }

    public async Task<bool> VerifyOtpAsync(VerifyOtpDto dto)
    {
        var tempUser = await _context.TempUsers.FirstOrDefaultAsync(t => t.Email == dto.Email);
        if (tempUser == null)
        {
            throw new InvalidOperationException("Verification record not found");
        }

        if (tempUser.ExpiresAt < DateTime.UtcNow)
        {
            _context.TempUsers.Remove(tempUser);
            await _context.SaveChangesAsync();
            throw new InvalidOperationException("OTP expired");
        }

        if (tempUser.VerificationCode != dto.Otp)
        {
            throw new InvalidOperationException("Invalid OTP code");
        }

        // Create actual user
        var user = new User
        {
            Email = tempUser.Email,
            Username = tempUser.Username,
            Password = tempUser.Password,
            IsEmailVerified = true,
            LastLoginAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        _context.TempUsers.Remove(tempUser);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> CancelRegisterAsync(string email)
    {
        var tempUser = await _context.TempUsers.FirstOrDefaultAsync(t => t.Email == email);
        if (tempUser == null)
        {
            throw new InvalidOperationException("Registration not found or already completed");
        }

        _context.TempUsers.Remove(tempUser);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto dto, string? userAgent, string? ipAddress)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null || user.Password == null)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        if (!_passwordHasher.VerifyPassword(user.Password, dto.Password))
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        var payload = CreateJwtPayload(user);
        var accessToken = _jwtService.GenerateAccessToken(payload);
        var refreshToken = _jwtService.GenerateRefreshToken(payload);

        // Save or update refresh token
        var expiresAt = DateTime.UtcNow.AddSeconds(
            int.Parse(_configuration["JwtSettings:RefreshTokenExpiresInSeconds"] ?? "2592000"));

        var existingToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.UserId == user.Id && rt.UserAgent == userAgent && !rt.Revoked);

        if (existingToken != null)
        {
            existingToken.Token = refreshToken;
            existingToken.ExpiresAt = expiresAt;
            existingToken.IpAddress = ipAddress;
        }
        else
        {
            var newToken = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                UserAgent = userAgent,
                IpAddress = ipAddress,
                ExpiresAt = expiresAt
            };
            _context.RefreshTokens.Add(newToken);
        }

        user.LastLoginAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return new LoginResponseDto(accessToken, refreshToken);
    }

    public async Task<LoginResponseDto> GoogleLoginAsync(string googleToken)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(googleToken, new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = new[] { _configuration["GoogleAuth:ClientId"] }
        });

        if (payload == null || string.IsNullOrEmpty(payload.Email))
        {
            throw new UnauthorizedAccessException("Invalid Google token");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == payload.Email);
        if (user == null)
        {
            // Create new user
            user = new User
            {
                Email = payload.Email,
                Username = payload.Name ?? payload.Email.Split('@')[0],
                Password = null, // No password for OAuth users
                Avatar = payload.Picture,
                IsEmailVerified = true,
                LastLoginAt = DateTime.UtcNow
            };
            _context.Users.Add(user);
        }
        else
        {
            user.LastLoginAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();

        var jwtPayload = CreateJwtPayload(user);
        var accessToken = _jwtService.GenerateAccessToken(jwtPayload);
        var refreshToken = _jwtService.GenerateRefreshToken(jwtPayload);

        // Save refresh token
        var expiresAt = DateTime.UtcNow.AddSeconds(
            int.Parse(_configuration["JwtSettings:RefreshTokenExpiresInSeconds"] ?? "2592000"));

        var tokenRecord = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            UserAgent = "Google-OAuth",
            ExpiresAt = expiresAt
        };
        _context.RefreshTokens.Add(tokenRecord);
        await _context.SaveChangesAsync();

        return new LoginResponseDto(accessToken, refreshToken);
    }

    public async Task LogoutAsync(Guid userId, string refreshToken)
    {
        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.UserId == userId && rt.Token == refreshToken);

        if (token != null)
        {
            token.Revoked = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<LoginResponseDto> RefreshTokenAsync(string refreshToken, string? ipAddress)
    {
        var token = await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        if (token == null || token.Revoked)
        {
            throw new UnauthorizedAccessException("Invalid or revoked token");
        }

        if (token.ExpiresAt < DateTime.UtcNow)
        {
            token.Revoked = true;
            await _context.SaveChangesAsync();
            throw new UnauthorizedAccessException("Refresh token expired");
        }

        var payload = CreateJwtPayload(token.User);
        var newAccessToken = _jwtService.GenerateAccessToken(payload);
        var newRefreshToken = _jwtService.GenerateRefreshToken(payload);

        // Rotate refresh token
        token.Token = newRefreshToken;
        token.ExpiresAt = DateTime.UtcNow.AddSeconds(
            int.Parse(_configuration["JwtSettings:RefreshTokenExpiresInSeconds"] ?? "2592000"));
        token.IpAddress = ipAddress;

        await _context.SaveChangesAsync();

        return new LoginResponseDto(newAccessToken, newRefreshToken);
    }

    private JwtPayload CreateJwtPayload(User user)
    {
        return new JwtPayload
        {
            UserId = user.Id,
            Name = user.Username,
            Email = user.Email,
            Role = user.Role.ToString(),
            Avatar = user.Avatar,
            Phone = user.PhoneNumber
        };
    }
}
