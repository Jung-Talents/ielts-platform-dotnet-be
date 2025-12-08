using IeltsPlatform.ApiService.DTOs.IeltsTest;
using IeltsPlatform.ApiService.DTOs.Test;
using IeltsPlatform.ApiService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IeltsPlatform.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IeltsTestController : Controller
    {
        private readonly IIeltsTestService _testService;
        public IeltsTestController(IIeltsTestService testService)
        {
            _testService = testService;
            // _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTest([FromBody] CreateIeltsTestRequest request, CancellationToken cancellation)
        {
            try
            {
                var createdTest = await _testService.CreateAsync(request, cancellation);
                return Ok(createdTest);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the test.", Error = ex.Message });
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<IeltsTestResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTests(CancellationToken cancellation)
        {
            var tests = await _testService.GetAllAsync(cancellation);
            return Ok(tests);
        }
    }
}
