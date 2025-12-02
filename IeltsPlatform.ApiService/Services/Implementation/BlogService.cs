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

        public async Task<BlogResponseDto> CreateAsync(CreateBlogRequest request, CancellationToken cancellationToken)
        {
            var blog = BlogMapper.CreateCategoryFromDto(request);
            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();
            return blog.ToResponseDto();
        }

        public async Task<BlogResponseDto> GetBlogByNameAsync(string blogName, CancellationToken cancellationToken)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Name == blogName, cancellationToken);
            if (blog == null)
                throw new KeyNotFoundException($"Blog with name '{blogName}' not found.");
            return blog.ToResponseDto();
        }

        public async Task<IEnumerable<BlogResponseDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var blogs = await _context.Blogs.ToListAsync(cancellationToken);
            return blogs.Select(b => b.ToResponseDto()).ToList();
        }

        public async Task<BlogResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var blog = await _context.Blogs.FindAsync(id, cancellationToken);
            return blog == null ? null : blog.ToResponseDto();
        }

        public async Task<BlogResponseDto?> UpdateAsync(Guid id, BlogUpdateDto dto, CancellationToken cancellationToken)
        {
            var blog = await _context.Blogs.FindAsync(id, cancellationToken);
            if (blog == null)
                return null;

            blog.ApplyUpdate(dto);

            await _context.SaveChangesAsync(cancellationToken);
            return blog.ToResponseDto();
        }


        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var blog = await _context.Blogs.FindAsync(id, cancellationToken);
            if (blog == null)
                return false;

            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
