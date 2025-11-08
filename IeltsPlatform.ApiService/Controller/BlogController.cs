using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IeltsPlatform.ApiService.Data;
using IeltsPlatform.ApiService.Entities;
using IeltsPlatform.ApiService.DTOs;

namespace IeltsPlatform.ApiService.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly BlogDbContext _context;
        private readonly ILogger<BlogController> _logger;

        public BlogController(BlogDbContext context, ILogger<BlogController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// Get all blogs (for testing purposes)
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Blog>>> GetAllBlogs()
        {
            _logger.LogInformation("Fetching all blogs");
            var blogs = await _context.Blogs
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return Ok(blogs);
        }

        /// Update an existing blog
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Blog>> UpdateBlog(Guid id, [FromBody] UpdateBlogRequest request)
        {
            try
            {
                // Validate ID
                if (id == Guid.Empty)
                {
                    _logger.LogWarning("Update attempted with empty GUID");
                    return BadRequest(new { error = "Invalid blog ID format" });
                }

                // Get existing blog
                var existingBlog = await _context.Blogs.FindAsync(id);

                if (existingBlog == null)
                {
                    _logger.LogInformation("Update attempted on non-existent blog: {BlogId}", id);
                    return NotFound(new { error = $"Blog with ID {id} not found" });
                }

                // Check if blog is soft-deleted
                if (existingBlog.DeletedAt.HasValue)
                {
                    _logger.LogWarning("Update attempted on deleted blog: {BlogId}", id);
                    return BadRequest(new
                    {
                        error = "Cannot update a deleted blog",
                        blogId = id,
                        deletedAt = existingBlog.DeletedAt
                    });
                }

                // Update blog properties
                existingBlog.BlogName = request.BlogName;
                existingBlog.BlogContent = request.BlogContent;
                existingBlog.Status = request.Status;
                existingBlog.Theme = request.Theme;
                existingBlog.UpdatedAt = DateTime.UtcNow;

                // Save changes to database
                _context.Blogs.Update(existingBlog);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Blog updated successfully: {BlogId}", id);
                return Ok(existingBlog);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error updating blog: {BlogId}", id);
                return StatusCode(500, new
                {
                    error = "A database error occurred while updating the blog",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating blog: {BlogId}", id);
                return StatusCode(500, new
                {
                    error = "An error occurred while updating the blog",
                    detail = ex.Message
                });
            }
        }
    }
}
