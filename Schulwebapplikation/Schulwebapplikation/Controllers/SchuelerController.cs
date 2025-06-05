using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Schulwebapplikation.Data;
using Schulwebapplikation.Models;

namespace Schulwebapplikation.Controllers
{
    [ApiController]
    [Route("api/schule")]
    public class SchuleController : ControllerBase
    {
        private readonly DBContext _context;

        public SchuleController(DBContext context)
        {
            _context = context;
        }

        [HttpPost("addSchueler")]
        public async Task<IActionResult> AddSchueler([FromBody] Schueler schueler)
        {
            if (schueler == null || string.IsNullOrEmpty(schueler.Name) || string.IsNullOrEmpty(schueler.Klasse))
            {
                return BadRequest("Schülerdaten fehlen oder sind ungültig.");
            }

            try
            {
                _context.Schueler.Add(schueler);
                await _context.SaveChangesAsync();
                return Ok("Schüler hinzugefügt!");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Fehler beim Hinzufügen: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpPost("addKlassenraum")]
        public async Task<IActionResult> AddKlassenraum([FromBody] Klassenraum klassenraum)
        {
            if (klassenraum == null || string.IsNullOrEmpty(klassenraum.Name))
            {
                return BadRequest("Klassenraumdaten fehlen oder sind ungültig.");
            }

            try
            {
                _context.Klassenraeume.Add(klassenraum);
                await _context.SaveChangesAsync();
                return Ok("Klassenraum hinzugefügt!");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Fehler beim Hinzufügen: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpGet("getAllSchueler")]
        public async Task<IActionResult> GetAllSchueler()
        {
            var schueler = await _context.Schueler.ToListAsync();
            return Ok(schueler);
        }

        [HttpGet("getSchuelerByKlasse/{klasse}")]
        public async Task<IActionResult> GetSchuelerByKlasse(string klasse)
        {
            var schuelerInKlasse = await _context.Schueler
                .Where(s => s.Klasse == klasse)
                .ToListAsync();
            return Ok(schuelerInKlasse);
        }

        [HttpGet("kannUnterrichten/{klasse}/{raumName}")]
        public async Task<IActionResult> KannUnterrichten(string klasse, string raumName)
        {
            var schuelerCount = await _context.Schueler
                .CountAsync(s => s.Klasse == klasse);
            var raum = await _context.Klassenraeume
                .FirstOrDefaultAsync(r => r.Name == raumName);

            if (raum == null)
            {
                return NotFound("Raum nicht gefunden.");
            }

            bool kannUnterrichten = raum.Plaetze >= schuelerCount;
            return Ok(kannUnterrichten ? "Ja, die Klasse kann unterrichtet werden." : "Nein, es gibt nicht genug Plätze.");
        }

        [HttpGet("durchschnittsalter")]
        public async Task<IActionResult> DurchschnittsalterSchueler()
        {
            var schueler = await _context.Schueler.ToListAsync();
            if (!schueler.Any())
            {
                return Ok(0);
            }
            double avgAge = schueler.Average(s => s.Alter);
            return Ok(avgAge);
        }

        [HttpGet("frauenanteil/{klasse}")]
        public async Task<IActionResult> BerechneFrauenanteilInProzent(string klasse)
        {
            var schuelerInKlasse = await _context.Schueler
                .Where(s => s.Klasse == klasse)
                .ToListAsync();
            if (!schuelerInKlasse.Any())
            {
                return Ok(0);
            }
            double frauenAnteil = (double)schuelerInKlasse.Count(s => s.Geschlecht == "weiblich") / schuelerInKlasse.Count * 100;
            return Ok(frauenAnteil);
        }
    }
}