using IeltsPlatform.Domain.Enums;

namespace IeltsPlatform.Domain.Entities;

public class IeltsTest
{
    public Guid Id { get; set; }
    public string TestName { get; set; } = string.Empty;
    public IeltsTestSkill Skill { get; set; }
    public short Duration { get; set; }
    public int? Order { get; set; }
    public IeltsTestStatus Status { get; set; } = IeltsTestStatus.Draft;
    public string Slug { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
