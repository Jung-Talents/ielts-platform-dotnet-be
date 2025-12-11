using IeltsPlatform.ApiService.Enums;
using System.ComponentModel.DataAnnotations;

namespace IeltsPlatform.ApiService.DTOs.IeltsTest
{
    public class CreateIeltsTestRequest
    {
        public required string TestName { get; init; }
        [Range(0, int.MaxValue, ErrorMessage = "Duration must be a positive integer.")]
        public required int Duration { get; init; }
        public required IeltsTestStatus Status { get; init; }
    }
}