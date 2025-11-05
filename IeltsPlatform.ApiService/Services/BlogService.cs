using IeltsPlatform.ApiService.Data;
using IeltsPlatform.ApiService.Enums;
using IeltsPlatform.ApiService.Models;
using IeltsPlatform.ApiService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IeltsPlatform.ApiService.Services
{
    public class BlogService : IBlogService
    {
        private readonly AppDbContext _context;

        public BlogService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Blog>> GetAllBlogsAsync(
            BlogTheme? theme = null,
            string sort = "default",
            int pageNumber = 1,
            int pageSize = 10)
        {
            IQueryable<Blog> query = _context.Blogs;

            // Filter by theme
            if (theme.HasValue)
            {
                query = query.Where(b => b.Theme == theme.Value);
            }

            // Sort by CreatedAt
            query = sort.ToLower() switch
            {
                "newest" => query.OrderByDescending(b => b.CreatedAt),
                "oldest" => query.OrderBy(b => b.CreatedAt),
                _ => query // default
            };

            // Pagination
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return await query.ToListAsync();
        }
    }
}
