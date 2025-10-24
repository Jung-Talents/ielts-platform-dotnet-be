using FluentValidation;
using IeltsPlatform.Domain.Enums;
using System.Text.Json;

namespace IeltsPlatform.Domain.DTOs.QuestionGroups;

public class CreateQuestionGroupDto
{
    public Guid SectionId { get; set; }
    public SectionCategory SectionType { get; set; }
    public QuestionType Type { get; set; }
    public int Order { get; set; }
}

public class UpdateQuestionGroupDto
{
    public Guid Id { get; set; }
    public string Instruction { get; set; } = string.Empty;
    public QuestionType Type { get; set; }
    public int Order { get; set; }
    public QuestionGroupCategory? Category { get; set; }
    public JsonDocument? Content { get; set; }
}

public class UploadImageDto
{
    public Guid GroupId { get; set; }
}

public class QuestionGroupResponseDto
{
    public Guid Id { get; set; }
}

public class ImageUploadResponseDto
{
    public string ImageUrl { get; set; } = string.Empty;
}

// Validators
public class CreateQuestionGroupDtoValidator : AbstractValidator<CreateQuestionGroupDto>
{
    public CreateQuestionGroupDtoValidator()
    {
        RuleFor(x => x.SectionId)
            .NotEmpty()
            .WithMessage("SectionId is required");

        RuleFor(x => x.SectionType)
            .IsInEnum()
            .WithMessage("SectionType must be valid");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Type must be valid");

        RuleFor(x => x.Order)
            .GreaterThan(0)
            .WithMessage("Order must be greater than 0");
    }
}

public class UpdateQuestionGroupDtoValidator : AbstractValidator<UpdateQuestionGroupDto>
{
    public UpdateQuestionGroupDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required");

        RuleFor(x => x.Instruction)
            .NotNull()
            .WithMessage("Instruction cannot be null");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Type must be valid");

        RuleFor(x => x.Order)
            .GreaterThan(0)
            .WithMessage("Order must be greater than 0");
    }
}
