using System.ComponentModel.DataAnnotations;
using IeltsPlatform.ApiService.Enums;
using IeltsPlatform.ApiService.Constants;

namespace IeltsPlatform.ApiService.DTOs.Blog
{
    public class BlogRequestDto
    {
        [Required(ErrorMessage = ValidationMessages.Required)]
        public string BlogName { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.Required)]
        public string BlogContent { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.Required)]
        [EnumDataType(typeof(BlogStatus), ErrorMessage = ValidationMessages.EnumInvalid)]
        public BlogStatus Status { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required)]
        [EnumDataType(typeof(BlogTheme), ErrorMessage = ValidationMessages.EnumInvalid)]
        public BlogTheme Theme { get; set; }
    }
}
