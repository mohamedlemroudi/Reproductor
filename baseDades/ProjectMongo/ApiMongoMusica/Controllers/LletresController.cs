using ApiMongoMusica.Classes.Models;
using ApiMongoMusica.Classes.Services;
using ApiMongoMusica.Classes;
using Microsoft.AspNetCore.Mvc;

namespace ApiMongoMusica.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LletresController : ControllerBase
    {
        private readonly LletresService _lletraService;

        public LletresController(LletresService lletraService)
        {
            _lletraService = lletraService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Lletres>>> GetAsync() =>
            await _lletraService.GetAsync();

        [HttpGet("{id:length(24)}", Name = "GetLletra")]
        public async Task<ActionResult<Lletres>> GetAsync(string id)
        {
            var lletra = await _lletraService.GetAsync(id);

            if (lletra == null)
            {
                return NotFound();
            }

            return lletra;
        }

        [HttpPost]
        public async Task<ActionResult<Lletres>> CreateAsync(Lletres lletra)
        {
            await _lletraService.CreateAsync(lletra);

            return CreatedAtRoute("GetLletra", new { id = lletra.Id.ToString() }, lletra);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateAsync(string id, Lletres lletraIn)
        {
            var lletra = await _lletraService.GetAsync(id);

            if (lletra == null)
            {
                return NotFound();
            }

            await _lletraService.UpdateAsync(id, lletraIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var lletra = await _lletraService.GetAsync(id);

            if (lletra == null)
            {
                return NotFound();
            }

            await _lletraService.RemoveAsync(lletra);

            return NoContent();
        }
    }
}