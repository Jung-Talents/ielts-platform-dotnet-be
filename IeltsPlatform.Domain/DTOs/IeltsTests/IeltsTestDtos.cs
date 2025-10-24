using FluentValidation;
using IeltsPlatform.Domain.Enums;

namespace IeltsPlatform.Domain.DTOs.IeltsTests;

public class CreateIeltsTestDto
{
    public string TestName { get; set; } = string.Empty;
    public IeltsTestSkill Skill { get; set; }
    public short Duration { get; set; }
    public int? Order { get; set; }
}

public class CreateIeltsTestDtoValidator : AbstractValidator<CreateIeltsTestDto>
{
    public CreateIeltsTestDtoValidator()
    {
        RuleFor(x => x.TestName)
            .NotEmpty().WithMessage("Test name is required")
            .MaximumLength(255).WithMessage("Test name must not exceed 255 characters");

        RuleFor(x => x.Skill)
            .IsInEnum().WithMessage("Invalid skill");

        RuleFor(x => x.Duration)
            .GreaterThan((short)0).WithMessage("Duration must be greater than 0");
    }
}

public class UpdateIeltsTestDto
{
    public string TestName { get; set; } = string.Empty;
    public short Duration { get; set; }
}

public class UpdateIeltsTestDtoValidator : AbstractValidator<UpdateIeltsTestDto>
{
    public UpdateIeltsTestDtoValidator()
    {
        RuleFor(x => x.TestName)
            .NotEmpty().WithMessage("Test name is required")
            .MaximumLength(255).WithMessage("Test name must not exceed 255 characters");

        RuleFor(x => x.Duration)
            .GreaterThan((short)0).WithMessage("Duration must be greater than 0");
    }
}

public class UpdateTestStatusDto
{
    public IeltsTestStatus Status { get; set; }
}

public class IeltsTestResponseDto
{
    public Guid Id { get; set; }
    public string TestName { get; set; } = string.Empty;
    public IeltsTestSkill Skill { get; set; }
    public short Duration { get; set; }
    public int? Order { get; set; }
    public IeltsTestStatus Status { get; set; }
    public string Slug { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class PaginatedIeltsTestsDto
{
    public List<IeltsTestResponseDto> Data { get; set; } = new();
    public PaginationMeta Meta { get; set; } = new();
}

public class PaginationMeta
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
    public int ItemsPerPage { get; set; }
}
