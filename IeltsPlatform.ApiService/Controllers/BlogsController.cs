using IeltsPlatform.ApiService.DTOs.Blog;
using IeltsPlatform.ApiService.Entities;
using IeltsPlatform.ApiService.Properties.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IeltsPlatform.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public BlogsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<ApiResult<List<Blog>>>> GetBlogs(CancellationToken cancellation)
        {
            try
            {
                var blogs = await _context.Blogs.ToListAsync(cancellation);
                return Ok(blogs);
            }
            catch
            {
                return StatusCode(500, "An error occurred while retrieving blogs.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlog([FromBody] CreateBlogRequest request, CancellationToken cancellation)
        {
            try
            {
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