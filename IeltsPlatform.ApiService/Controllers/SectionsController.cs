using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IeltsPlatform.ApiService.Data;

namespace IeltsPlatform.ApiService.Controllers
{
    [ApiController]
    [Route("sections")]
    public class SectionsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public SectionsController(AppDbContext db)
        {
            _db = db;
        }

        // DELETE /sections/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteSection([FromRoute] Guid id)
        {
            var section = await _db.Sections.FirstOrDefaultAsync(s => s.Id == id);
            if (section == null)
                return NotFound();

            // Soft delete by setting DeletedAt or remove from Db
            // Here we'll remove the record
            _db.Sections.Remove(section);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}