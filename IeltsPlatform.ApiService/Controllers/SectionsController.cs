using IeltsPlatform.Domain.Common;
using IeltsPlatform.Domain.DTOs.Sections;
using IeltsPlatform.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IeltsPlatform.ApiService.Controllers;

[ApiController]
[Route("sections")]
[Authorize(Roles = "Admin,Moderator")]
public class SectionsController : ControllerBase
{
    private readonly ISectionsService _sectionsService;

    public SectionsController(ISectionsService sectionsService)
    {
        _sectionsService = sectionsService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<SectionResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SectionResponseDto>>> CreateSection([FromBody] CreateSectionDto dto)
    {
        try
        {
            var sectionId = await _sectionsService.CreateSectionAsync(dto);
            return Ok(ApiResponse<SectionResponseDto>.SuccessResponse(
                new SectionResponseDto { Id = sectionId },
                "IELTS test section created successfully"
            ));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<SectionResponseDto>.FailureResponse(ex.Message));
        }
    }

    [HttpPut("transcript")]
    [ProducesResponseType(typeof(ApiResponse<SectionResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SectionResponseDto>>> UpdateSectionTranscript([FromBody] UpdateSectionDto dto)
    {
        try
        {
            var sectionId = await _sectionsService.UpdateSectionTranscriptAsync(dto);
            return Ok(ApiResponse<SectionResponseDto>.SuccessResponse(
                new SectionResponseDto { Id = sectionId },
                "Section updated successfully"
            ));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<SectionResponseDto>.FailureResponse(ex.Message));
        }
    }

    [HttpPut("audio")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(ApiResponse<SectionResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SectionResponseDto>>> UploadSectionAudio(
        [FromForm] UploadAudioDto dto,
        IFormFile audio)
    {
        try
        {
            if (audio == null || audio.Length == 0)
            {
                return BadRequest(ApiResponse<SectionResponseDto>.FailureResponse("Audio file is required"));
            }

            using var stream = audio.OpenReadStream();
            var url = await _sectionsService.UploadSectionAudioAsync(dto.Id, stream, audio.FileName);

            return Ok(ApiResponse<SectionResponseDto>.SuccessResponse(
                new SectionResponseDto { Id = dto.Id },
                "Section audio uploaded successfully"
            ));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<SectionResponseDto>.FailureResponse(ex.Message));
        }
    }

    [HttpDelete]
    [ProducesResponseType(typeof(ApiResponse<SectionResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SectionResponseDto>>> DeleteSection([FromBody] DeleteSectionDto dto)
    {
        try
        {
            var sectionId = await _sectionsService.DeleteSectionAsync(dto);
            return Ok(ApiResponse<SectionResponseDto>.SuccessResponse(
                new SectionResponseDto { Id = sectionId },
                "Section deleted successfully"
            ));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<SectionResponseDto>.FailureResponse(ex.Message));
        }
    }
}
