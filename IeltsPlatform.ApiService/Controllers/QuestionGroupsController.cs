using IeltsPlatform.Domain.Common;
using IeltsPlatform.Domain.DTOs.QuestionGroups;
using IeltsPlatform.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IeltsPlatform.ApiService.Controllers;

[ApiController]
[Route("question-groups")]
[Authorize(Roles = "Admin,Moderator")]
public class QuestionGroupsController : ControllerBase
{
    private readonly IQuestionGroupsService _questionGroupsService;

    public QuestionGroupsController(IQuestionGroupsService questionGroupsService)
    {
        _questionGroupsService = questionGroupsService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<QuestionGroupResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<QuestionGroupResponseDto>>> CreateQuestionGroup([FromBody] CreateQuestionGroupDto dto)
    {
        try
        {
            var groupId = await _questionGroupsService.CreateQuestionGroupAsync(dto);
            return Ok(ApiResponse<QuestionGroupResponseDto>.SuccessResponse(
                new QuestionGroupResponseDto { Id = groupId },
                "Question group created successfully"
            ));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<QuestionGroupResponseDto>.FailureResponse(ex.Message));
        }
    }

    [HttpPost("images/upload")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(ApiResponse<ImageUploadResponseDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<ImageUploadResponseDto>>> UploadImage(
        [FromForm] UploadImageDto dto,
        IFormFile image)
    {
        try
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest(ApiResponse<ImageUploadResponseDto>.FailureResponse("Image file is required"));
            }

            using var stream = image.OpenReadStream();
            var imageUrl = await _questionGroupsService.UploadQuestionImageAsync(dto.GroupId, stream, image.FileName);

            return StatusCode(StatusCodes.Status201Created, 
                ApiResponse<ImageUploadResponseDto>.SuccessResponse(
                    new ImageUploadResponseDto { ImageUrl = imageUrl },
                    "Image uploaded successfully"
                ));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<ImageUploadResponseDto>.FailureResponse(ex.Message));
        }
    }

    [HttpDelete("images/{groupId}")]
    [ProducesResponseType(typeof(ApiResponse<ImageUploadResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ImageUploadResponseDto>>> RemoveImage(Guid groupId)
    {
        try
        {
            var imageUrl = await _questionGroupsService.RemoveQuestionImageAsync(groupId);
            return Ok(ApiResponse<ImageUploadResponseDto>.SuccessResponse(
                new ImageUploadResponseDto { ImageUrl = imageUrl ?? string.Empty },
                "Image removed successfully"
            ));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<ImageUploadResponseDto>.FailureResponse(ex.Message));
        }
    }

    [HttpPut]
    [ProducesResponseType(typeof(ApiResponse<QuestionGroupResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<QuestionGroupResponseDto>>> UpdateQuestionGroup([FromBody] UpdateQuestionGroupDto dto)
    {
        try
        {
            var groupId = await _questionGroupsService.UpdateQuestionGroupAsync(dto);
            return Ok(ApiResponse<QuestionGroupResponseDto>.SuccessResponse(
                new QuestionGroupResponseDto { Id = groupId },
                "Question group updated successfully"
            ));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<QuestionGroupResponseDto>.FailureResponse(ex.Message));
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<QuestionGroupResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<QuestionGroupResponseDto>>> DeleteQuestionGroup(Guid id)
    {
        try
        {
            var groupId = await _questionGroupsService.DeleteQuestionGroupAsync(id);
            return Ok(ApiResponse<QuestionGroupResponseDto>.SuccessResponse(
                new QuestionGroupResponseDto { Id = groupId },
                "Question group deleted successfully"
            ));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<QuestionGroupResponseDto>.FailureResponse(ex.Message));
        }
    }
}
