namespace IeltsPlatform.ApiService.DTOs.IeltsTest
{
    public class IeltsTestMapper
    {
        public static Entities.IeltsTest CreateIeltsTestFromDto(CreateIeltsTestRequest dto)
        {
            return Entities.IeltsTest.Create(
                dto.Name,
                dto.Duration,
                dto.Status
            );
        }
    }
}
