namespace IeltsPlatform.ApiService.Entities
{
    public class Blog
    {
        public Blog() { }
        public Blog(string blog_name, string blog_content, Status blog_status, Theme blog_theme)
        {
            Id = Guid.NewGuid();
            Blog_name = blog_name;
            Blog_content = blog_content;
            Blog_status = blog_status;
            Blog_theme = blog_theme;
        }
        public static Blog Create(string blog_name, string blog_content, Status blog_status, Theme blog_theme)
        {
            return new Blog(blog_name, blog_content, blog_status, blog_theme);
        }
        public Guid Id { get; set; }
        public string Blog_name { get; set; }
        public string Blog_content { get; set; }
        public enum Status
        {
            Draft,
            Published,
            Archived
        }
        public Status Blog_status { get; set; }
        public enum Theme
        {
            Listening,
            Reading,
            Speaking,
            Writing
        }
        public Theme Blog_theme { get; set; }
        public DateTimeOffset? Updated_at { get; set; }
        public DateTimeOffset Created_at { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? Deleted_at { get; set; }
    }
}
