using Microsoft.AspNetCore.Mvc;

namespace IeltsPlatform.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeathersController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetWeather()
        {
            return Ok("Đây là một Controller mẫu để test. Thời tiết hôm nay: Nắng đẹp, nhiệt độ 25°C!");
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("API đang hoạt động bình thường!");
        }
    }
}

