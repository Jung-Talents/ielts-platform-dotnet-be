using IeltsPlatform.ApiService.Data;
using IeltsPlatform.ApiService.DTOs.IeltsTest;
using IeltsPlatform.ApiService.DTOs.Test;
using IeltsPlatform.ApiService.Mappings;
using IeltsPlatform.ApiService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IeltsPlatform.ApiService.Services.Implementation
{
    public class IeltsTestService : IIeltsTestService
    {
        private readonly AppDbContext _context;

        public IeltsTestService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IeltsTestResponseDto> CreateAsync(CreateIeltsTestRequest request, CancellationToken cancellationToken)
        {
            var test = IeltsTestMapper.CreateIeltsTestFromDto(request);
            _context.IeltsTests.Add(test);
            await _context.SaveChangesAsync(cancellationToken);
            return test.ToResponseDto();
        }
        public async Task<IEnumerable<IeltsTestResponseDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var test = await _context.IeltsTests.ToListAsync(cancellationToken);
            return test.Select(b => b.ToResponseDto()).ToList();
        }
    }
}
