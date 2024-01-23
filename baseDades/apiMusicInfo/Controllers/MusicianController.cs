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
    public class MusicianController : ControllerBase
    {
        private readonly DataContext _context;

        private readonly MusicianService _MusicianService;

        public MusicianController(DataContext context)
        {
            _context = context;

            _MusicianService = new MusicianService(context);
        }

        // GET: api/Musician
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Musician>>> GetMusician()
        {
            return await _MusicianService.GetMusicians();
        }

        // GET: api/Musician/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Musician>> GetMusician(string id)
        {
            var musician = await _MusicianService.GetMusician(id);

            if (musician == null)
            {
                return NotFound();
            }

            return Ok(musician);
        }

        // PUT: api/Musician/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMusician(string id, Musician musician)
        {
            if (id != musician.Name)
            {
                return BadRequest();
            }

            var result = await _MusicianService.PutMusician(id, musician);

            return Ok(result);
        }

        //PUT: api/Musician/Alhuerto/RosaMelano
        [HttpPut("{nameMusician}/{nameBand}")]
        public async Task<IActionResult> AddMusicianBand(string nameMusician, string nameBand)
        {
            var result = await _MusicianService.AddMusicianBand(nameMusician, nameBand);

            if (!result.IsSuccess)
            {
                if (result.Message != null)
                {
                    if (result.Message.Contains("not found"))
                    {
                        return NotFound(result.Message);
                    }
                    else if (result.Message.Contains("already associated"))
                    {
                        return Conflict(result.Message);
                    }
                    else
                    {
                        return BadRequest(result.Message);
                    }
                }
                else
                {
                    return BadRequest();
                }
            }

            return Ok(result.Message); // or any other appropriate success response
        }
        // POST: api/Musician
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Musician>> PostMusician(Musician musician)
        {
            _context.Musician.Add(musician);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MusicianExists(musician.Name))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMusician", new { id = musician.Name }, musician);
        }

        // DELETE: api/Musician/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMusician(string id)
        {
            var musician = await _context.Musician.FindAsync(id);
            if (musician == null)
            {
                return NotFound();
            }

            _context.Musician.Remove(musician);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MusicianExists(string id)
        {
            return _context.Musician.Any(e => e.Name == id);
        }
    }
}
