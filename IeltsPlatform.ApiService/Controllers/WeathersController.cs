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
            return Ok("Hello. Today is a sunny day!");
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("API đang hoạt động bình thường!");
        }
    }
}

