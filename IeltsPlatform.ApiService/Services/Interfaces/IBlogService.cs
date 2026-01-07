using IeltsPlatform.ApiService.DTOs.Blog;

namespace IeltsPlatform.ApiService.Services.Interfaces
{
    public interface IBlogService
    {
        Task<IEnumerable<BlogResponseDto>> GetAllAsync();
        Task<BlogResponseDto?> GetByIdAsync(Guid id);
        Task<BlogResponseDto?> UpdateAsync(Guid id, BlogUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
