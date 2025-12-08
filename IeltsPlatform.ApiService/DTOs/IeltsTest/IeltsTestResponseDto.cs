using IeltsPlatform.ApiService.Enums.IeltsTest;

namespace IeltsPlatform.ApiService.DTOs.Test
{
    public record IeltsTestResponseDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required IeltsTestStatus Status { get; set; }
        public required int Duration { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
