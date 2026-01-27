using IeltsPlatform.ApiService.Enums;
using System.ComponentModel.DataAnnotations;

namespace IeltsPlatform.ApiService.DTOs.IeltsTest
{
    public class IeltsTestUpdateDto
    {
        public string? TestName { get; set; }
        public int? Duration { get; set; }

        [EnumDataType(typeof(IeltsTestStatus))]
        public IeltsTestStatus? Status { get; set; }
    }
}