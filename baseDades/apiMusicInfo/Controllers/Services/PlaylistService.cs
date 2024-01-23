using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apiMusicInfo.Data;
using apiMusicInfo.Models;
using Azure.Core.Pipeline;


namespace apiMusicInfo.Controllers.Services
{
    public class PlaylistService
    {
        private readonly DataContext _context;

        public PlaylistService(DataContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<Playlist>>> GetPlaylists()
        {
            return await _context.Playlists.ToListAsync();
        }

        public async Task<ActionResult<Playlist>?> GetPlaylist(string Dispositiu)
        {
            var playlist = await _context.Playlists.FindAsync(Dispositiu);

            if (playlist == null)
            {
                return null;
            }

            return playlist;
        }

        public async Task<IActionResult?> PutPlaylist(string Dispositiu, Playlist playlist)
        {
            bool PlaylistExists(string Dispositiu)
            {
                return _context.Playlists.Any(p => p.Dispositiu == Dispositiu);
            }

            if (Dispositiu != playlist.Dispositiu)
            {
                return null;
            }

            _context.Entry(playlist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!PlaylistExists(Dispositiu))
            {
                return null;
            }

            return null;
        }


        public async Task<ActionResult<Playlist>?> PostPlaylist(Playlist playlist)
        {
            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();

            return null;
        }

        public async Task<ActionResult<Playlist>?> DeletePlaylist(string Dispositiu)
        {
            var playlist = await _context.Playlists.FindAsync(Dispositiu);
            if (playlist == null)
            {
                return null;
            }

            _context.Playlists.Remove(playlist);
            await _context.SaveChangesAsync();

            return null;
        }
    }
}