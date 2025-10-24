using IeltsPlatform.Domain.Enums;
using System.Text.Json;

namespace IeltsPlatform.Domain.Entities;

public class QuestionGroup
{
    public Guid Id { get; set; }
    public Guid SectionId { get; set; }
    public SectionCategory SectionType { get; set; }
    public string Instruction { get; set; } = string.Empty;
    public QuestionType Type { get; set; }
    public short Order { get; set; }
    public QuestionGroupCategory? Category { get; set; }
    public string? Image { get; set; }
    public JsonDocument? Content { get; set; }
    public DateTime? DeletedAt { get; set; }
}
