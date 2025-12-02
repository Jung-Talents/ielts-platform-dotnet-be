using IeltsPlatform.ApiService.DTOs.Blog;
using IeltsPlatform.ApiService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IeltsPlatform.ApiService.Services.Interfaces;

namespace IeltsPlatform.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogsController(IBlogService blogService)
        {
            _blogService = blogService;
            // _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlog([FromBody] CreateBlogRequest request, CancellationToken cancellation)
        {
            try
            {
                var createdBlog = await _blogService.CreateAsync(request, cancellation);
                return Ok(createdBlog);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the blog.", Error = ex.Message });
            }
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BlogResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBlogs(CancellationToken cancellation)
        {
            var blogs = await _blogService.GetAllAsync(cancellation);
            return Ok(blogs);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BlogResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBlogById(Guid id, CancellationToken cancellation)
        {
            var blog = await _blogService.GetByIdAsync(id, cancellation);
            if (blog == null) return NotFound();
            return Ok(blog);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BlogResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateBlog(Guid id, [FromBody] BlogUpdateDto dto, CancellationToken cancellation)
        {
            var updated = await _blogService.UpdateAsync(id, dto, cancellation);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<IActionResult> DeleteBlog(Guid id, CancellationToken cancellation)
        {
            var success = await _blogService.DeleteAsync(id, cancellation);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}