using IeltsPlatform.Domain.Common;
using IeltsPlatform.Domain.DTOs.TestResults;
using IeltsPlatform.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IeltsPlatform.ApiService.Controllers;

[ApiController]
[Route("ielts-test-results")]
[Authorize]
public class IeltsTestResultsController : ControllerBase
{
    private readonly ITestResultsService _testResultsService;

    public IeltsTestResultsController(ITestResultsService testResultsService)
    {
        _testResultsService = testResultsService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<TestResultResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<TestResultResponseDto>>> CreateTestResult([FromBody] CreateTestResultDto dto)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty);
            var result = await _testResultsService.CreateTestResultAsync(userId, dto);
            return Ok(ApiResponse<TestResultResponseDto>.SuccessResponse(result, "IELTS test result created successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<TestResultResponseDto>.FailureResponse(ex.Message));
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<TestResultDetailDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<TestResultDetailDto>>> GetTestResultById(Guid id)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty);
            var result = await _testResultsService.GetTestResultByIdAsync(userId, id);
            return Ok(ApiResponse<TestResultDetailDto>.SuccessResponse(result, "IELTS test result retrieved successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<TestResultDetailDto>.FailureResponse(ex.Message));
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PaginatedTestResultsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedTestResultsDto>>> GetTestResults(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 10)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty);
        var results = await _testResultsService.GetTestResultsByUserIdAsync(userId, page, limit);
        return Ok(ApiResponse<PaginatedTestResultsDto>.SuccessResponse(results, "IELTS tests result fetched successfully"));
    }
}
