using IeltsPlatform.ApiService.DTOs.IeltsTest;
using IeltsPlatform.ApiService.DTOs.Test;

namespace IeltsPlatform.ApiService.Services.Interfaces
{
    public interface IIeltsTestService
    {
        Task<IeltsTestResponseDto> CreateAsync(CreateIeltsTestRequest request, CancellationToken cancellation);
        Task<IEnumerable<IeltsTestResponseDto>> GetAllAsync(CancellationToken cancellationToken);
    }
}
