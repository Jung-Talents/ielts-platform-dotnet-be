using IeltsPlatform.ApiService.DTOs.IeltsTest;
using IeltsPlatform.ApiService.Entities;

namespace IeltsPlatform.ApiService.Mappings
{
    public static class IeltsTestMapping
    {
        public static IeltsTest ToEntity(this CreateIeltsTestRequest dto)
        {
            return IeltsTest.Create(
                dto.TestName,
                dto.Duration,
                dto.Status
            );
        }

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

        public static void ApplyUpdate(this IeltsTest test, IeltsTestUpdateDto dto)
        {
            if (dto.TestName != null)
                test.TestName = dto.TestName;

            if (dto.Duration.HasValue)
                test.Duration = dto.Duration.Value;

            if (dto.Status.HasValue)
                test.Status = dto.Status.Value;

            test.UpdatedAt = DateTime.UtcNow;
        }
    }
}
