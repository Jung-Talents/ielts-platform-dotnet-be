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
        public async Task<ActionResult<IEnumerable<Blog>>> GetAllBlogs()
        {
            var blogs = await _blogService.GetAllBlogsAsync();
            return Ok(blogs);
        }
    }
}
