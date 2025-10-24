using IeltsPlatform.Domain.Common;
using IeltsPlatform.Domain.DTOs.Users;
using IeltsPlatform.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Amazon.S3;
using Amazon.S3.Model;

namespace IeltsPlatform.ApiService.Controllers;

[ApiController]
[Route("users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAmazonS3 _s3Client;
    private readonly IConfiguration _configuration;

    public UsersController(
        IUserService userService,
        IAmazonS3 s3Client,
        IConfiguration configuration)
    {
        _userService = userService;
        _s3Client = s3Client;
        _configuration = configuration;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<UserDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<UserDto>>>> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(ApiResponse<List<UserDto>>.SuccessResponse(users, "Users retrieved successfully"));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetUserById(Guid id)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(ApiResponse<UserDto>.SuccessResponse(user, "User retrieved successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<UserDto>.FailureResponse(ex.Message));
        }
    }

    [HttpGet("profile")]
    [ProducesResponseType(typeof(ApiResponse<UserProfileDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<UserProfileDto>>> GetProfile()
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty);
            var profile = await _userService.GetUserProfileAsync(userId);
            return Ok(ApiResponse<UserProfileDto>.SuccessResponse(profile));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<UserProfileDto>.FailureResponse(ex.Message));
        }
    }

    [HttpPut("update")]
    [ProducesResponseType(typeof(ApiResponse<UserProfileDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<UserProfileDto>>> UpdateProfile([FromBody] UpdateUserDto dto)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty);
            var updatedProfile = await _userService.UpdateUserAsync(userId, dto);
            return Ok(ApiResponse<UserProfileDto>.SuccessResponse(updatedProfile, "Profile updated successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<UserProfileDto>.FailureResponse(ex.Message));
        }
    }

    [HttpPost("upload/user-image")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<string>>> UploadUserImage(IFormFile image)
    {
        try
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest(ApiResponse<string>.FailureResponse("Image file is required"));
            }

            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty);

            // Upload to S3
            var bucketName = _configuration["AWS:BucketName"];
            var key = $"user-images/{userId}-{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";

            using var stream = image.OpenReadStream();
            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = key,
                InputStream = stream,
                ContentType = image.ContentType
            };

            await _s3Client.PutObjectAsync(request);

            var url = $"https://{bucketName}.s3.amazonaws.com/{key}";
            await _userService.UpdateUserAvatarAsync(userId, url);

            return Ok(ApiResponse<string>.SuccessResponse(url, "Image uploaded successfully"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<string>.FailureResponse($"Failed to upload image: {ex.Message}"));
        }
    }

    [HttpPut("role")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<UserDto>>> UpdateUserRole([FromBody] UpdateUserRoleDto dto)
    {
        try
        {
            var updatedUser = await _userService.UpdateUserRoleAsync(dto.Id, dto.Role);
            return Ok(ApiResponse<UserDto>.SuccessResponse(updatedUser, "User role updated successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<UserDto>.FailureResponse(ex.Message));
        }
    }
}
