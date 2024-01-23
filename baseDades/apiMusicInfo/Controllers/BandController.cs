using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apiMusicInfo.Data;
using apiMusicInfo.Models;
using apiMusicInfo.Controllers.Services;

namespace apiMusicInfo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BandController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly BandService _bandService;

        public BandController(DataContext context)
        {
            _context = context;
            _bandService = new BandService(context);
        }

        // GET: api/Band
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Band>>> GetBand()
        {
            return await _bandService.GetBand();
        }

        // GET: api/Band/5
        [HttpGet("{name}")]
        public async Task<ActionResult<Band>> GetBand(string name)
        {
            var band = await _bandService.GetBand(name);
            if (band == null)
            {
                return NotFound();
            }

            return band;
        }

        // PUT: api/Band/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBand(string id, Band band)
        {
            if (id != band.Name)
            {
                return BadRequest();
            }

            var result = await _bandService.PutBand(id, band);

            return Ok(result);
        }

        // POST: api/Band
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostBand(Band band)
        {
            var result = await _bandService.PostBand(band);

            return CreatedAtAction("GetBand", new { id = band.Name }, band);
        }

        // DELETE: api/Band/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBand(string id)
        {
            var result = await _bandService.DeleteBand(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
