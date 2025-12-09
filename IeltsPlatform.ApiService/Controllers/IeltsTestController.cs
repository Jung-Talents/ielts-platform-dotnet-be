using IeltsPlatform.ApiService.DTOs.IeltsTest;
using IeltsPlatform.ApiService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

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
        }

        [HttpPost]
        public async Task<IActionResult> CreateTest([FromBody] CreateIeltsTestRequest request, CancellationToken cancellation)
        {
            var createdTest = await _testService.CreateAsync(request, cancellation);
            return CreatedAtAction(nameof(GetTestById), new { id = createdTest.Id }, new
            {
                Message = $"{createdTest.TestName} created successfully!",
                Data = createdTest
            });
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<IeltsTestResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTests(CancellationToken cancellation)
        {
            var test = await _testService.GetAllAsync(cancellation);
            return Ok(test);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IeltsTestResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTestById(Guid id, CancellationToken cancellation)
        {
            // Implement this method in IIeltsTestService and IeltsTestService
            var test = await _testService.GetByIdAsync(id, cancellation);
            return test == null ? NotFound() : Ok(test);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteTest(Guid id, CancellationToken cancellation)
        {
            var success = await _testService.DeleteAsync(id, cancellation);
            if (!success)
                return NotFound();
            return Ok(new { Message = "IELTS Test deleted successfully!" });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(IeltsTestResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateTest(Guid id, [FromBody] IeltsTestUpdateDto dto, CancellationToken cancellation)
        {
            var updated = await _testService.UpdateAsync(id, dto, cancellation);
            if (updated == null)
                return NotFound();
            return Ok(updated);
        }
    }
}