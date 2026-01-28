using Microsoft.AspNetCore.Mvc;
using IeltsPlatform.ApiService.Data;
using IeltsPlatform.ApiService.Dtos;
using IeltsPlatform.ApiService.Models;

namespace IeltsPlatform.ApiService.Controllers
{
    [ApiController]
    [Route("/sections")]
    public class SectionsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public SectionsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSection([FromBody] CreateSectionDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                return BadRequest(new { error = "Title is required" });
            }

            var section = new Section
            {
                Id = Guid.NewGuid(),
                Title = dto.Title.Trim(),
                Description = dto.Description?.Trim(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.Sections.Add(section);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSection), new { id = section.Id }, section);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetSection(Guid id)
        {
            var section = await _db.Sections.FindAsync(id);
            if (section == null) return NotFound();
            return Ok(section);
        }
    }
}