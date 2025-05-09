using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Schulwebapplikation.Models;

namespace Schulwebapplikation.Controllers
{
    [ApiController]
    [Route("api/schule")]
    public class SchuleController : ControllerBase
    {
        private static Schule schule = new Schule();

        [HttpPost("addSchueler")]
        public IActionResult AddSchueler([FromBody] Schueler schueler)
        {
            if (schueler == null)
            {
                return BadRequest("Schülerdaten fehlen.");
            }
            try
            {
                schule.AddSchuelerToSchule(schueler);
                return Ok("Schüler hinzugefügt!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Fehler beim Hinzufügen: {ex.Message}");
            }
        }

        [HttpGet("getAllSchueler")]
        public IActionResult GetAllSchueler()
        {
            return Ok(schule.SchuelerList);
        }

        [HttpGet("getSchuelerByKlasse/{klasse}")]
        public IActionResult GetSchuelerByKlasse(string klasse)
        {
            var schuelerInKlasse = schule.SchuelerList.Where(s => s.Klasse == klasse).ToList();
            return Ok(schuelerInKlasse);
        }

        [HttpGet("kannUnterrichten/{klasse}/{raumName}")]
        public IActionResult KannUnterrichten(string klasse, string raumName)
        {
            bool kannUnterrichten = schule.KannKlasseUnterrichten(klasse, raumName);
            return Ok(kannUnterrichten ? "Ja, die Klasse kann unterrichtet werden." : "Nein, es gibt nicht genug Plätze.");
        }
    }
}