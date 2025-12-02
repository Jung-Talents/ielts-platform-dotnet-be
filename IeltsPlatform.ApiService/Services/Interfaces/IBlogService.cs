using IeltsPlatform.ApiService.DTOs.Blog;

namespace IeltsPlatform.ApiService.Services.Interfaces
{
    public interface IBlogService
    {
        Task<BlogResponseDto> CreateAsync(CreateBlogRequest request, CancellationToken cancellationToken);
        Task<BlogResponseDto> GetBlogByNameAsync(string blogName, CancellationToken cancellationToken);
        Task<IEnumerable<BlogResponseDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<BlogResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<BlogResponseDto?> UpdateAsync(Guid id, BlogUpdateDto dto, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
