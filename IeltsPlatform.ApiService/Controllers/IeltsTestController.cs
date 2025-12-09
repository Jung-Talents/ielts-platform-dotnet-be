using IeltsPlatform.ApiService.DTOs.IeltsTest;
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
                // Implement CreatedAtAction(nameof(GetTestById)... after adding GetTestById method
                return StatusCode(201, new
                {
                    Message = "IELTS Test created successfully!",
                    Data = createdTest
                });
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