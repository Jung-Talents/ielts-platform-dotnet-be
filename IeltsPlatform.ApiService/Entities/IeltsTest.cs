using IeltsPlatform.ApiService.Enums;

namespace IeltsPlatform.ApiService.Entities
{
    public sealed class IeltsTest
    {
        private IeltsTest() { }
        
        private IeltsTest(
            string testName,
            int duration,
            IeltsTestStatus status
            )
        {
            Id = Guid.NewGuid();
            TestName = testName;
            Duration = duration;
            Status = status;
        }

        public static IeltsTest Create(
            string testName,
            int duration,
            IeltsTestStatus status
            )
        {
            var now = DateTimeOffset.UtcNow;
            return new IeltsTest(testName, duration, status)
            {
                CreatedAt = now,
                UpdatedAt = now
            };
        }

        public Guid Id { get; set; }
        public string TestName { get; set; } = string.Empty;
        public int Duration { get; set; }
        public IeltsTestStatus Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? DeletedAt { get; set; }
        
        // Navigation property
        public ICollection<Section> Sections { get; set; } = new List<Section>();
    }
}

