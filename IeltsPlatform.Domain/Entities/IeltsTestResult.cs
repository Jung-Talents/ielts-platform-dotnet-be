using System.Text.Json;

namespace IeltsPlatform.Domain.Entities;

public class IeltsTestResult
{
    public Guid Id { get; set; }
    public Guid TestId { get; set; }
    public Guid UserId { get; set; }
    public float Score { get; set; }
    public short TotalCorrectAnswers { get; set; }
    public JsonDocument UserSubmission { get; set; } = null!;
    public JsonDocument DetailAnalysis { get; set; } = null!;
    public float TimeSpent { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public IeltsTest Test { get; set; } = null!;
    public User User { get; set; } = null!;
}
