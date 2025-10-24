using FluentValidation;
using IeltsPlatform.Domain.Enums;

namespace IeltsPlatform.Domain.DTOs.Users;

public class UserProfileDto
{
    public string? Avatar { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string Email { get; set; } = string.Empty;
}

public class UserDto
{
    public Guid Id { get; set; }
    public string? Avatar { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public bool IsEmailVerified { get; set; }
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
}

public class UpdateUserDto
{
    public string? Username { get; set; }
    public string? PhoneNumber { get; set; }
}

public class UpdateUserRoleDto
{
    public Guid Id { get; set; }
    public UserRole Role { get; set; }
}

// Validators
public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator()
    {
        When(x => !string.IsNullOrEmpty(x.Username), () =>
        {
            RuleFor(x => x.Username)
                .MinimumLength(3)
                .WithMessage("Username must be at least 3 characters");
        });

        When(x => !string.IsNullOrEmpty(x.PhoneNumber), () =>
        {
            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?[1-9]\d{1,14}$")
                .WithMessage("Phone number must be valid");
        });
    }
}

public class UpdateUserRoleDtoValidator : AbstractValidator<UpdateUserRoleDto>
{
    public UpdateUserRoleDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("User Id is required");

        RuleFor(x => x.Role)
            .IsInEnum()
            .WithMessage("Role must be valid");
    }
}
