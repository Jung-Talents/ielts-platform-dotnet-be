using Microsoft.AspNetCore.Mvc;

namespace IeltsPlatform.ApiService.DTOs.Blog
{
    public record CreateBlogRequest
    {
        public required string Blog_name { get; init; }
        public required string Blog_content { get; init; }
        public required Entities.Blog.Status Blog_status { get; init; }
        public required Entities.Blog.Theme Blog_theme { get; init; }
    }
}
