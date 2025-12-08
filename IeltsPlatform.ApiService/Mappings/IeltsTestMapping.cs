using IeltsPlatform.ApiService.DTOs.Test;
using IeltsPlatform.ApiService.Entities;

namespace IeltsPlatform.ApiService.Mappings
{
    public static class IeltsTestMapping
    {
        public static IeltsTestResponseDto ToResponseDto(this IeltsTest entity)
        {
            return new IeltsTestResponseDto
            {
                Id = entity.Id,
                TestName = entity.TestName,
                Status = entity.Status,
                Duration = entity.Duration,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }
    }
}
