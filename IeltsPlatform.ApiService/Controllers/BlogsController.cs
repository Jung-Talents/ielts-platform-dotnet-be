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
        private readonly AppDbContext _context;
        public BlogsController(IBlogService blogService, AppDbContext context)
        {
            _blogService = blogService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlog([FromBody] CreateBlogRequest request, CancellationToken cancellation)
        {
            // no same title
            try
            {
                var existTitle = await _context.Blogs.AnyAsync(blog => blog.Name == request.Name, cancellation);
                if (existTitle)
                {
                    return Conflict(new { Message = "A blog with the same title already exists." });
                }
                var createdBlog = BlogMapper.CreateCategoryFromDto(request);
                _context.Blogs.Add(createdBlog);
                await _context.SaveChangesAsync(cancellation);
                return Ok(new { Message = "Blog created successfully", Name = createdBlog.Name.ToString(), Theme = createdBlog.Theme.ToString(), Status = createdBlog.Status.ToString() });
            }
            catch
            {
                return StatusCode(500, "An error occurred while creating the blog.");
            }
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