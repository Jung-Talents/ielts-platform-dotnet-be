using IeltsPlatform.ApiService.Data;
using IeltsPlatform.ApiService.Entities;
using Microsoft.AspNetCore.Mvc;
using IeltsPlatform.ApiService.Enums;
using Microsoft.EntityFrameworkCore;

namespace IeltsPlatform.ApiService.Controllers
{
    public class BlogController
    {
        private readonly BlogDbContext _context;

        public BlogController(BlogDbContext context)
        {
            _context = context;
        }

        //GET /blogs
        [HttpGet("blogs")]
        public async Task<ActionResult<IEnumerable<Blog>>> GetBlogs(
            [FromQuery] int page = 1,

            [FromQuery] int limit = 10,

            [FromQuery] BlogTheme? theme = null
        )
        {
            var query = _context.Blogs.AsQueryable();

            if (theme.HasValue)
            {
                query = query.Where(b => b.Theme == theme.Value);
            }

            var blogs = await query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
                
            return blogs;
        }
    }
}