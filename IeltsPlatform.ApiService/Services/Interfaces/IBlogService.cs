using IeltsPlatform.ApiService.Models;

namespace IeltsPlatform.ApiService.Services.Interfaces
{
    public interface IBlogService
    {
        Task<IEnumerable<Blog>> GetAllBlogsAsync();
    }
}
