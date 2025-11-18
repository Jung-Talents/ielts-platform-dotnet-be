using System.ComponentModel.DataAnnotations;
using IeltsPlatform.ApiService.Enums;

namespace IeltsPlatform.ApiService.DTOs.Blog
{
    public class BlogUpdateDto
    {
        public string? BlogName { get; set; }
        public string? BlogContent { get; set; }

        [EnumDataType(typeof(BlogStatus))]
        public BlogStatus? Status { get; set; }

        [EnumDataType(typeof(BlogTheme))]
        public BlogTheme? Theme { get; set; }
    }
}
