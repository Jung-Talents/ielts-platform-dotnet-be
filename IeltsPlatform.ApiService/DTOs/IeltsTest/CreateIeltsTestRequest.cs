using IeltsPlatform.ApiService.Enums.IeltsTest;

namespace IeltsPlatform.ApiService.DTOs.IeltsTest
{
    public class CreateIeltsTestRequest
    {
        public required string Name { get; init; }
        public required int Duration { get; init; }
        public required IeltsTestStatus Status { get; init; }
    }
}