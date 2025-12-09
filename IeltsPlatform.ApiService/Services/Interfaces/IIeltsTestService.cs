using IeltsPlatform.ApiService.DTOs.IeltsTest;

namespace IeltsPlatform.ApiService.Services.Interfaces
{
    public interface IIeltsTestService
    {
        Task<IeltsTestResponseDto> CreateAsync(CreateIeltsTestRequest request, CancellationToken cancellation);
        Task<IEnumerable<IeltsTestResponseDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<IeltsTestResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<IeltsTestResponseDto?> UpdateAsync(Guid id, IeltsTestUpdateDto dto, CancellationToken cancellationToken);
    }
}
