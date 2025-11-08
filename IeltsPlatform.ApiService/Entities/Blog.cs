namespace IeltsPlatform.ApiService.Entities
{
    public class Blog
    {
        public Guid Id { get; set; }
        public string BlogName { get; set; } = string.Empty;
        public string BlogContent { get; set; } = string.Empty;
        public BlogStatus Status { get; set; }
        public BlogTheme Theme { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
