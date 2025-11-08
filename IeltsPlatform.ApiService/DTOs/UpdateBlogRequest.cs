using IeltsPlatform.ApiService.Entities;

namespace IeltsPlatform.ApiService.DTOs
{
    public class UpdateBlogRequest
    {
        public string BlogName { get; set; } = string.Empty;
        public string BlogContent { get; set; } = string.Empty;
        public BlogStatus Status { get; set; }
        public BlogTheme Theme { get; set; }
    }
}
