using IeltsPlatform.ApiService.Enums.IeltsTest;

namespace IeltsPlatform.ApiService.Entities
{
    public sealed class IeltsTest
    {
        private IeltsTest() { }
        private IeltsTest(string test_name, int duration, IeltsTestStatus status)
        {
            Id = Guid.NewGuid();
            Name = test_name;
            Duration = duration;
            Status = status;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }
        public static IeltsTest Create(string test_name, int duration, IeltsTestStatus status)
        {
            return new IeltsTest(test_name, duration, status);
        }
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Duration { get; set; }
        public IeltsTestStatus Status { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; } = null;
    }
}