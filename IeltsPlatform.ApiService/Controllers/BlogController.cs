using Microsoft.AspNetCore.Mvc;
using IeltsPlatform.ApiService.Services.Interfaces;
using IeltsPlatform.ApiService.DTOs.Blog;

namespace IeltsPlatform.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BlogResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBlogs()
        {
            var blogs = await _blogService.GetAllAsync();
            return Ok(blogs);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BlogResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBlogById(Guid id)
        {
            var blog = await _blogService.GetByIdAsync(id);
            if (blog == null) return NotFound();
            return Ok(blog);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlog([FromBody] BlogRequestDto dto)
        {
            var created = await _blogService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetBlogById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BlogResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateBlog(Guid id, [FromBody] BlogUpdateDto dto)
        {
            var updated = await _blogService.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<IActionResult> DeleteBlog(Guid id)
        {
            var success = await _blogService.DeleteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
