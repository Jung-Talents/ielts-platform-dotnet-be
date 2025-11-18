using IeltsPlatform.ApiService.Entities;
using IeltsPlatform.ApiService.Data;
using Microsoft.EntityFrameworkCore;
using IeltsPlatform.ApiService.DTOs.Blog;
using IeltsPlatform.ApiService.Services.Interfaces;
using IeltsPlatform.ApiService.Mappings;

namespace IeltsPlatform.ApiService.Services.Implementations
{
    public class BlogService : IBlogService
    {
        private readonly AppDbContext _context;

        public BlogService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BlogResponseDto>> GetAllAsync()
        {
            var blogs = await _context.Blogs.ToListAsync();
            return blogs.Select(b => b.ToResponseDto()).ToList();
        }

        public async Task<BlogResponseDto?> GetByIdAsync(Guid id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            return blog == null ? null : blog.ToResponseDto();
        }

        public async Task<BlogResponseDto?> UpdateAsync(Guid id, BlogUpdateDto dto)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
                return null;

            blog.ApplyUpdate(dto);

            await _context.SaveChangesAsync();
            return blog.ToResponseDto();
        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
                return false;

            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
