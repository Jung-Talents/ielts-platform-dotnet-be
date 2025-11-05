using IeltsPlatform.ApiService.Enums;
using IeltsPlatform.ApiService.Models;

namespace IeltsPlatform.ApiService.Services.Interfaces
{
    public interface IBlogService
    {
        Task<IEnumerable<Blog>> GetAllBlogsAsync(
            BlogTheme? theme = null,
            string sort = "default",
            int pageNumber = 1,
            int pageSize = 10);
    }
}
