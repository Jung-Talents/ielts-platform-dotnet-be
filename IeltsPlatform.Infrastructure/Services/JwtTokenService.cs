using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IeltsPlatform.Infrastructure.Services;

public interface IJwtTokenService
{
    string GenerateAccessToken(JwtPayload payload);
    string GenerateRefreshToken(JwtPayload payload);
    ClaimsPrincipal? ValidateToken(string token);
}

public class JwtPayload
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public string? Phone { get; set; }
}

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;
    private readonly string _secret;
    private readonly int _accessTokenExpiry;
    private readonly int _refreshTokenExpiry;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
        _secret = configuration["JwtSettings:Secret"] ?? throw new InvalidOperationException("JWT Secret not configured");
        _accessTokenExpiry = int.Parse(configuration["JwtSettings:AccessTokenExpiresInSeconds"] ?? "900");
        _refreshTokenExpiry = int.Parse(configuration["JwtSettings:RefreshTokenExpiresInSeconds"] ?? "2592000");
    }

    public string GenerateAccessToken(JwtPayload payload)
    {
        return GenerateToken(payload, _accessTokenExpiry);
    }

    public string GenerateRefreshToken(JwtPayload payload)
    {
        return GenerateToken(payload, _refreshTokenExpiry);
    }

    private string GenerateToken(JwtPayload payload, int expirySeconds)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secret);
        
        var claims = new List<Claim>
        {
            new Claim("sub", payload.UserId.ToString()),
            new Claim("name", payload.Name),
            new Claim("email", payload.Email),
            new Claim("role", payload.Role)
        };

        if (!string.IsNullOrEmpty(payload.Avatar))
            claims.Add(new Claim("avatar", payload.Avatar));

        if (!string.IsNullOrEmpty(payload.Phone))
            claims.Add(new Claim("phone", payload.Phone));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddSeconds(expirySeconds),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secret);

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return principal;
        }
        catch
        {
            return null;
        }
    }
}
