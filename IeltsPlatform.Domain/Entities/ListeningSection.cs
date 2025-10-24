namespace IeltsPlatform.Domain.Entities;

public class ListeningSection
{
    public Guid Id { get; set; }
    public Guid TestId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public short Order { get; set; }
    public string Audio { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    // Navigation property
    public IeltsTest Test { get; set; } = null!;
}
