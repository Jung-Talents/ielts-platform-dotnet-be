using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IeltsPlatform.ApiService.Data;

namespace IeltsPlatform.ApiService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SectionsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public SectionsController(AppDbContext db)
        {
            _db = db;
        }

        // PUT /sections/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateSection([FromRoute] Guid id, [FromBody] UpdateSectionRequest request)
        {
            var section = await _db.Sections.FirstOrDefaultAsync(s => s.Id == id);
            if (section == null)
                return NotFound();

            // Update allowed fields
            if (!string.IsNullOrWhiteSpace(request.Title))
                section.Title = request.Title;

            section.Description = request.Description; // allow null to clear description
            section.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return NoContent();
        }
    }

    public record UpdateSectionRequest
    {
        public string? Title { get; init; }
        public string? Description { get; init; }
    }
}