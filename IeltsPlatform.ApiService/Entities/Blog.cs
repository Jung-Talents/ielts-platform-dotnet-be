namespace IeltsPlatform.ApiService.Entities
{
    public class Blog
    {
        public Blog() { }
        public Blog(string blog_name, string blog_content, Status blog_status, Theme blog_theme)
        {
            Id = Guid.NewGuid();
            BlogName = blog_name;
            BlogContent = blog_content;
            BlogStatus = blog_status;
            BlogTheme = blog_theme;
        }
        public static Blog Create(string blog_name, string blog_content, Status blog_status, Theme blog_theme)
        {
            return new Blog(blog_name, blog_content, blog_status, blog_theme);
        }
        public Guid Id { get; set; }
        public string BlogName { get; set; }
        public string BlogContent { get; set; }
        public enum Status
        {
            Draft,
            Published,
            Archived
        }
        public Status BlogStatus { get; set; }
        public enum Theme
        {
            Listening,
            Reading,
            Speaking,
            Writing
        }
        public Theme BlogTheme { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
