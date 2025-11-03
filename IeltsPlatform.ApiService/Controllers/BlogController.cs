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
            [FromQuery] int page = 1, // Page number for pagination (default: 1)
            [FromQuery] int limit = 10, // Number of results per page (default: 10)
            [FromQuery] BlogTheme? theme = null // Optional filter by blog theme
        )
        {
            // Build the query for blogs
            var query = _context.Blogs.AsQueryable();

            // If a theme filter is provided, apply it
            if (theme.HasValue)
            {
                query = query.Where(b => b.Theme == theme.Value);
            }

            // Apply pagination: skip (page-1)*limit items, take 'limit' items
            var blogs = await query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            // Return the paginated and filtered list of blogs
            return blogs;
        }
    }
}