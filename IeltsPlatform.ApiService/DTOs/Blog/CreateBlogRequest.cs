using IeltsPlatform.ApiService.Enums;

namespace IeltsPlatform.ApiService.DTOs.Blog
{
    public record CreateBlogRequest
    {
        public required string Name { get; init; }
        public required string Content { get; init; }
        public BlogStatus Status { get; init; }
        public required BlogTheme Theme { get; init; }
    }
}
