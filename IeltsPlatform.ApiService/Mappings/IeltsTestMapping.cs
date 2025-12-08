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
                Name = entity.Name,
                Status = entity.Status,
                Duration = entity.Duration,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }
    }
}
