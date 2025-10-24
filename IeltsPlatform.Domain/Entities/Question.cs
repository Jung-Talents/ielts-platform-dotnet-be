using System.Text.Json;

namespace IeltsPlatform.Domain.Entities;

public class Question
{
    public Guid Id { get; set; }
    public Guid GroupId { get; set; }
    public string Title { get; set; } = string.Empty;
    public JsonDocument Content { get; set; } = null!;
    public string? Image { get; set; }
    public short Order { get; set; }
    public DateTime? DeletedAt { get; set; }

    // Navigation property
    public QuestionGroup Group { get; set; } = null!;
}
