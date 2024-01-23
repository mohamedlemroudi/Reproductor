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
    public class PlaylistController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly PlaylistService _playlistService;

        public PlaylistController(DataContext context)
        {
            _context = context;
            _playlistService = new PlaylistService(context);
        }

        // GET: api/Playlist
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Playlist>>> GetPlaylists()
        {
            return await _playlistService.GetPlaylists();
        }

        // GET: api/Playlist/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Playlist>> GetPlaylist(string Dispositiu)
        {
            var playlist = await _playlistService.GetPlaylist(Dispositiu);

            if (playlist == null)
            {
                return NotFound();
            }

            return playlist;
        }

        // PUT: api/Playlist/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlaylist(string Dispositiu, Playlist playlist)
        {
            var result = await _playlistService.PutPlaylist(Dispositiu, playlist);

            if (result == null)
            {
                return BadRequest();
            }

            return NoContent();
        }

        // POST: api/Playlist
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Playlist>> PostPlaylist(Playlist playlist)
        {
            await _playlistService.PostPlaylist(playlist);

            return CreatedAtAction("GetPlaylist", new { id = playlist.Dispositiu }, playlist);
        }

        // DELETE: api/Playlist/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaylist(string id)
        {
            var playlist = await _playlistService.GetPlaylist(id);
            if (playlist == null)
            {
                return NotFound();
            }

            await _playlistService.DeletePlaylist(id);

            return NoContent();
        }
    }
}
