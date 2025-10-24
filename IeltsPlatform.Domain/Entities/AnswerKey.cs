using System.Text.Json;

namespace IeltsPlatform.Domain.Entities;

public class AnswerKey
{
    public Guid Id { get; set; }
    public Guid QuestionId { get; set; }
    public Guid TestId { get; set; }
    public JsonDocument Answers { get; set; } = null!;
    public string? Clarification { get; set; }
    public DateTime? DeletedAt { get; set; }

    // Navigation properties
    public Question Question { get; set; } = null!;
    public IeltsTest Test { get; set; } = null!;
}
