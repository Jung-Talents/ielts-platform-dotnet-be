using FluentValidation;
using IeltsPlatform.Domain.Enums;

namespace IeltsPlatform.Domain.DTOs.Sections;

public class CreateSectionDto
{
    public Guid TestId { get; set; }
    public int Order { get; set; }
}

public class UpdateSectionDto
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
}

public class DeleteSectionDto
{
    public Guid SectionId { get; set; }
    public SectionCategory SectionType { get; set; }
}

public class UploadAudioDto
{
    public Guid Id { get; set; }
}

public class SectionResponseDto
{
    public Guid Id { get; set; }
}

// Validators
public class CreateSectionDtoValidator : AbstractValidator<CreateSectionDto>
{
    public CreateSectionDtoValidator()
    {
        RuleFor(x => x.TestId)
            .NotEmpty()
            .WithMessage("TestId is required");

        RuleFor(x => x.Order)
            .GreaterThan(0)
            .WithMessage("Order must be greater than 0");
    }
}

public class UpdateSectionDtoValidator : AbstractValidator<UpdateSectionDto>
{
    public UpdateSectionDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Section Id is required");

        RuleFor(x => x.Description)
            .NotNull()
            .WithMessage("Description cannot be null");
    }
}

public class DeleteSectionDtoValidator : AbstractValidator<DeleteSectionDto>
{
    public DeleteSectionDtoValidator()
    {
        RuleFor(x => x.SectionId)
            .NotEmpty()
            .WithMessage("SectionId is required");

        RuleFor(x => x.SectionType)
            .IsInEnum()
            .WithMessage("SectionType must be one of: Listening, Reading, Writing");
    }
}
