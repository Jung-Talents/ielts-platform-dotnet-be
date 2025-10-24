namespace IeltsPlatform.Domain.Entities;

public class ReadingSection
{
    public Guid Id { get; set; }
    public Guid TestId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public short Order { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    // Navigation property
    public IeltsTest Test { get; set; } = null!;
}
