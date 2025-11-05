using IeltsPlatform.ApiService.Data;
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

        public async Task<IEnumerable<Blog>> GetAllBlogsAsync()
        {
            return await _context.Blogs.ToListAsync();
        }
    }
}
