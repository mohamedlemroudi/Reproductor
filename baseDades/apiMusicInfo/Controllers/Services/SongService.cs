using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apiMusicInfo.Data;
using apiMusicInfo.Models;

namespace apiMusicInfo.Controllers.Services
{
    public class SongService
    {
        private readonly DataContext _context;

        public SongService(DataContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<Song>>> GetSongs()
        {
            return await _context.Songs.ToListAsync();
        }

        public async Task<IEnumerable<Song>?> GetSong(string uid)
        {
            Guid guidUid = Guid.Parse(uid);
            var song = await _context.Songs.Where(s => s.UID == guidUid).ToListAsync();

            if (song == null)
            {
                return null;
            }

            return null;
        }

        public async Task<IActionResult?> PutSong(Guid uid, Song song)
        {
            if (uid != song.UID)
            {
                return null;
            }

            _context.Entry(song).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }

        public async Task<ActionResult<Song>?> PostSong(Song song)
        {
            _context.Songs.Add(song);
            await _context.SaveChangesAsync();

            return song;
        }
        
        public async Task<ActionResult<Song>?> DeleteSong(Guid UID)
        {
            var song = await _context.Songs.FindAsync(UID);
            if (song == null)
            {
                return null;
            }
            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();

            return song;
        }
    }
}