using IeltsPlatform.ApiService.Enums;

namespace IeltsPlatform.ApiService.DTOs.Blog
{
    public class BlogResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public BlogStatus Status { get; set; }
        public BlogTheme Theme { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
