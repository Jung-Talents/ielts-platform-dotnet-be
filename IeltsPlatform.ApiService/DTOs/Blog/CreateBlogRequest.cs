using Microsoft.AspNetCore.Mvc;

namespace IeltsPlatform.ApiService.DTOs.Blog
{
    public record CreateBlogRequest
    {
        public required string BlogName { get; init; }
        public required string BlogContent { get; init; }
        public Entities.Blog.Status BlogStatus { get; init; }
        public required Entities.Blog.Theme BlogTheme { get; init; }
    }
}
