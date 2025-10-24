using IeltsPlatform.Domain.DTOs.Users;
using IeltsPlatform.Domain.Enums;
using IeltsPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IeltsPlatform.Infrastructure.Services;

public interface IUserService
{
    Task<UserProfileDto> GetUserProfileAsync(Guid userId);
    Task<List<UserDto>> GetAllUsersAsync();
    Task<UserDto> GetUserByIdAsync(Guid userId);
    Task<UserProfileDto> UpdateUserAsync(Guid userId, UpdateUserDto dto);
    Task UpdateUserAvatarAsync(Guid userId, string avatarUrl);
    Task<UserDto> UpdateUserRoleAsync(Guid userId, UserRole role);
}

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserProfileDto> GetUserProfileAsync(Guid userId)
    {
        var user = await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => new UserProfileDto
            {
                Avatar = u.Avatar,
                Username = u.Username,
                PhoneNumber = u.PhoneNumber,
                Email = u.Email
            })
            .FirstOrDefaultAsync();

        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        return user;
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        var users = await _context.Users
            .OrderByDescending(u => u.CreatedAt)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Avatar = u.Avatar,
                Username = u.Username,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                IsEmailVerified = u.IsEmailVerified,
                Role = u.Role,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt,
                LastLoginAt = u.LastLoginAt
            })
            .ToListAsync();

        return users;
    }

    public async Task<UserDto> GetUserByIdAsync(Guid userId)
    {
        var user = await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Avatar = u.Avatar,
                Username = u.Username,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                IsEmailVerified = u.IsEmailVerified,
                Role = u.Role,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt,
                LastLoginAt = u.LastLoginAt
            })
            .FirstOrDefaultAsync();

        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        return user;
    }

    public async Task<UserProfileDto> UpdateUserAsync(Guid userId, UpdateUserDto dto)
    {
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        if (!string.IsNullOrEmpty(dto.Username))
        {
            user.Username = dto.Username;
        }

        if (!string.IsNullOrEmpty(dto.PhoneNumber))
        {
            user.PhoneNumber = dto.PhoneNumber;
        }

        await _context.SaveChangesAsync();

        return new UserProfileDto
        {
            Avatar = user.Avatar,
            Username = user.Username,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email
        };
    }

    public async Task UpdateUserAvatarAsync(Guid userId, string avatarUrl)
    {
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        user.Avatar = avatarUrl;
        await _context.SaveChangesAsync();
    }

    public async Task<UserDto> UpdateUserRoleAsync(Guid userId, UserRole role)
    {
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        user.Role = role;
        await _context.SaveChangesAsync();

        return new UserDto
        {
            Id = user.Id,
            Avatar = user.Avatar,
            Username = user.Username,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            IsEmailVerified = user.IsEmailVerified,
            Role = user.Role,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            LastLoginAt = user.LastLoginAt
        };
    }
}
