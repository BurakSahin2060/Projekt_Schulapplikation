using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Schulwebapplikation.Data;
using Schulwebapplikation.Models;

namespace Schulwebapplikation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchuelerController : ControllerBase
    {
        private readonly SchulDbContext _context;

        public SchuelerController(SchulDbContext context)
        {
            _context = context;
        }

        // GET: api/Schueler
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Schueler>>> GetSchueler()
        {
            return await _context.Schueler.ToListAsync();
        }

        // GET: api/Schueler/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Schueler>> GetSchueler(int id)
        {
            var schueler = await _context.Schueler.FindAsync(id);
            if (schueler == null)
            {
                return NotFound();
            }
            return schueler;
        }

        // POST: api/Schueler
        [HttpPost]
        public async Task<ActionResult<Schueler>> CreateSchueler(Schueler schueler)
        {
            _context.Schueler.Add(schueler);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSchueler), new { id = schueler.Id }, schueler);
        }

        // PUT: api/Schueler/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchueler(int id, Schueler schueler)
        {
            if (id != schueler.Id)
            {
                return BadRequest();
            }

            _context.Entry(schueler).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Schueler.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }
            return NoContent();
        }

        // DELETE: api/Schueler/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchueler(int id)
        {
            var schueler = await _context.Schueler.FindAsync(id);
            if (schueler == null)
            {
                return NotFound();
            }

            _context.Schueler.Remove(schueler);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}