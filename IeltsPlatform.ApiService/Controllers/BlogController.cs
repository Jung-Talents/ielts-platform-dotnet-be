using IeltsPlatform.ApiService.Enums;
using IeltsPlatform.ApiService.Models;
using IeltsPlatform.ApiService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IeltsPlatform.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Blog>>> GetAllBlogs(
            [FromQuery] string? theme = null,
            [FromQuery] string sort = "default",
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            //BlogTheme? blogTheme = theme != null && Enum.TryParse<BlogTheme>(theme, true, out var parsedTheme) ? parsedTheme : null;

            BlogTheme? blogTheme = null;

            if (!string.IsNullOrEmpty(theme))
            {
                bool isValidTheme = Enum.TryParse<BlogTheme>(theme, true, out var parsedTheme);
                if (isValidTheme)
                {
                    blogTheme = parsedTheme;
                }
            }

            var blogs = await _blogService.GetAllBlogsAsync(blogTheme, sort, pageNumber, pageSize);

            return Ok(blogs);
        }
    }
}
