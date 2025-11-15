namespace IeltsPlatform.ApiService.Entities
{
    public sealed class Blog
    {
        private Blog() { }
        private Blog(string name, string content, BlogStatus status, BlogTheme theme)
        {
            Id = Guid.NewGuid();
            Name = name;
            Content = content;
            Status = status;
            Theme = theme;
        }
        public static Blog Create(string name, string content, BlogStatus status, BlogTheme theme)
        {
            return new Blog(name, content, status, theme);
        }
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public BlogStatus Status { get; set; }
        public BlogTheme Theme { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
