using IeltsPlatform.ApiService.DTOs.Blog;
using IeltsPlatform.ApiService.Entities;
using IeltsPlatform.ApiService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IeltsPlatform.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public BlogsController(AppDbContext context)
        {
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
                    return Conflict(new {Message = "A blog with the same title already exists." });
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
    }
}