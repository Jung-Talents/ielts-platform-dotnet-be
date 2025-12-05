namespace IeltsPlatform.ApiService.Entities
{
    public sealed class Section
    {
        private Section() { }
        
        private Section(
            int sectionNumber,
            string audioLink,
            string transcript,
            Guid ieltsTestId,
            DateTimeOffset createdAt,
            DateTimeOffset? updatedAt)
        {
            Id = Guid.NewGuid();
            SectionNumber = sectionNumber;
            AudioLink = audioLink;
            Transcript = transcript;
            IeltsTestId = ieltsTestId;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public static Section Create(
            int sectionNumber,
            string audioLink,
            string transcript,
            Guid ieltsTestId,
            DateTimeOffset createdAt,
            DateTimeOffset? updatedAt)
        {
            return new Section(sectionNumber, audioLink, transcript, ieltsTestId, createdAt, updatedAt);
        }

        public Guid Id { get; set; }
        public int SectionNumber { get; set; }
        public string AudioLink { get; set; } = string.Empty;
        public string Transcript { get; set; } = string.Empty;
        public Guid IeltsTestId { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? DeletedAt { get; set; }
        
        // Navigation property
        public IeltsTest IeltsTest { get; set; } = null!;
    }
}

