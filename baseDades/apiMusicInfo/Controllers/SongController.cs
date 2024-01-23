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
    public class SongController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly SongService _SongService;


        public SongController(DataContext context)
        {
            _context = context;
            _SongService = new SongService(context);
        }
        // GET: api/Song
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Song>>> GetSongs()
        {
            return await _SongService.GetSongs();
        }

        // GET: api/Song/5
        [HttpGet("{UID}")]
        public async Task<IEnumerable<Song>?> GetSong(Guid UID)
        {
            var song = await _SongService.GetSong(UID.ToString());

            if (song == null)
            {
                return null;
            }

            return song;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSong(Guid id, Song song)
        {
            if (id != song.UID)
            {
                return BadRequest();
            }

            await _SongService.PutSong(id, song);

            return NoContent();
        }
        
        // POST: api/Song
        [HttpPost]
        public async Task<ActionResult<Song>> PostSong(Song song)
        {
            await _SongService.PostSong(song);

            string stringUID = song.UID.ToString();

            return CreatedAtAction("GetSong", new { UID = stringUID }, song);
        }

        // DELETE: api/Song/5
        [HttpDelete("{UID}")]
        public async Task<IActionResult> DeleteSong(Guid UID)
        {
            var deletedSong = await _SongService.DeleteSong(UID);

            if (deletedSong == null)
            {
                return NotFound();
            }


            return Ok(deletedSong);
        }
    }
}
