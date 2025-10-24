using IeltsPlatform.Domain.Common;
using IeltsPlatform.Domain.DTOs.IeltsTests;
using IeltsPlatform.Domain.Enums;
using IeltsPlatform.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IeltsPlatform.ApiService.Controllers;

[ApiController]
[Route("ielts-tests")]
public class IeltsTestsController : ControllerBase
{
    private readonly IIeltsTestService _testService;

    public IeltsTestsController(IIeltsTestService testService)
    {
        _testService = testService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Moderator")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<Guid>>> CreateTest([FromBody] CreateIeltsTestDto dto)
    {
        try
        {
            var id = await _testService.CreateTestAsync(dto);
            return Ok(ApiResponse<Guid>.SuccessResponse(id, "Test created successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ApiResponse<Guid>.FailureResponse(ex.Message));
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PaginatedIeltsTestsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedIeltsTestsDto>>> GetTests(
        [FromQuery] IeltsTestSkill skill,
        [FromQuery] int page = 1,
        [FromQuery] int limit = 10)
    {
        var userRole = User.FindFirst("role")?.Value;
        var result = await _testService.GetTestsBySkillAsync(skill, page, limit, userRole);
        return Ok(ApiResponse<PaginatedIeltsTestsDto>.SuccessResponse(result));
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Moderator")]
    [ProducesResponseType(typeof(ApiResponse<IeltsTestResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IeltsTestResponseDto>>> GetTestById(Guid id)
    {
        var test = await _testService.GetTestByIdAsync(id);
        if (test == null)
        {
            return NotFound(ApiResponse<IeltsTestResponseDto>.FailureResponse("Test not found"));
        }

        return Ok(ApiResponse<IeltsTestResponseDto>.SuccessResponse(test));
    }

    [HttpGet("slug/{slug}")]
    [ProducesResponseType(typeof(ApiResponse<IeltsTestResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IeltsTestResponseDto>>> GetTestBySlug(string slug)
    {
        var test = await _testService.GetTestBySlugAsync(slug);
        if (test == null)
        {
            return NotFound(ApiResponse<IeltsTestResponseDto>.FailureResponse("Test not found"));
        }

        return Ok(ApiResponse<IeltsTestResponseDto>.SuccessResponse(test));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Moderator")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<Guid>>> UpdateTest(Guid id, [FromBody] UpdateIeltsTestDto dto)
    {
        try
        {
            var testId = await _testService.UpdateTestAsync(id, dto);
            return Ok(ApiResponse<Guid>.SuccessResponse(testId, "Test updated successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<Guid>.FailureResponse(ex.Message));
        }
    }

    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Admin,Moderator")]
    [ProducesResponseType(typeof(ApiResponse<IeltsTestResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IeltsTestResponseDto>>> UpdateTestStatus(
        Guid id, 
        [FromBody] UpdateTestStatusDto dto)
    {
        try
        {
            var test = await _testService.UpdateTestStatusAsync(id, dto.Status);
            var response = new IeltsTestResponseDto
            {
                Id = test.Id,
                TestName = test.TestName,
                Skill = test.Skill,
                Duration = test.Duration,
                Order = test.Order,
                Status = test.Status,
                Slug = test.Slug,
                CreatedAt = test.CreatedAt,
                UpdatedAt = test.UpdatedAt
            };
            return Ok(ApiResponse<IeltsTestResponseDto>.SuccessResponse(response, "Status updated successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<IeltsTestResponseDto>.FailureResponse(ex.Message));
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<Guid>>> DeleteTest(Guid id)
    {
        try
        {
            var testId = await _testService.DeleteTestAsync(id);
            return Ok(ApiResponse<Guid>.SuccessResponse(testId, "Test deleted successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<Guid>.FailureResponse(ex.Message));
        }
    }
}
