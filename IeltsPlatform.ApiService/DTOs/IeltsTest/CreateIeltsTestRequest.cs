using IeltsPlatform.ApiService.Enums;

namespace IeltsPlatform.ApiService.DTOs.IeltsTest
{
    public class CreateIeltsTestRequest
    {
        public required string TestName { get; init; }
        public required int Duration { get; init; }
        public required IeltsTestStatus Status { get; init; }
    }
}