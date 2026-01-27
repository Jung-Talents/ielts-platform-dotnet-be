using IeltsPlatform.ApiService.Enums;

namespace IeltsPlatform.ApiService.DTOs.IeltsTest
{
    public record IeltsTestResponseDto
    {
        public Guid Id { get; set; }
        public required string TestName { get; set; }
        public required IeltsTestStatus Status { get; set; }
        public required int Duration { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
