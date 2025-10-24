using IeltsPlatform.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Amazon.S3;
using Amazon.S3.Model;

namespace IeltsPlatform.ApiService.Controllers;

[ApiController]
[Route("aws")]
[Authorize]
public class AwsController : ControllerBase
{
    private readonly IAmazonS3 _s3Client;
    private readonly IConfiguration _configuration;

    public AwsController(IAmazonS3 s3Client, IConfiguration configuration)
    {
        _s3Client = s3Client;
        _configuration = configuration;
    }

    [HttpPost("presigned-url")]
    [ProducesResponseType(typeof(ApiResponse<PresignedUrlResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PresignedUrlResponse>>> GetPresignedUrl([FromBody] PresignedUrlRequest request)
    {
        try
        {
            var bucketName = _configuration["AWS:BucketName"];
            var key = $"{request.Folder}/{Guid.NewGuid()}{Path.GetExtension(request.FileName)}";

            var presignedRequest = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = key,
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddMinutes(15),
                ContentType = request.ContentType
            };

            var url = await _s3Client.GetPreSignedURLAsync(presignedRequest);

            var response = new PresignedUrlResponse
            {
                Url = url,
                Key = key,
                BucketName = bucketName
            };

            return Ok(ApiResponse<PresignedUrlResponse>.SuccessResponse(response, "Presigned URL generated successfully"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<PresignedUrlResponse>.FailureResponse($"Failed to generate presigned URL: {ex.Message}"));
        }
    }
}

public class PresignedUrlRequest
{
    public string FileName { get; set; } = string.Empty;
    public string Folder { get; set; } = "uploads";
    public string ContentType { get; set; } = "application/octet-stream";
}

public class PresignedUrlResponse
{
    public string Url { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string BucketName { get; set; } = string.Empty;
}
