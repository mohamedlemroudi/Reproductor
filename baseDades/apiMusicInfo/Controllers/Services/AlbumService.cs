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
    public class AlbumService
    {
        private readonly DataContext _context;

        public AlbumService(DataContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<Album>>> GetAlbums()
        {
            return await _context.Albums.ToListAsync();
        }

        public async Task<List<Album>?> GetAlbums(string AlbumName, int year)
        {
            var albums = await _context.Albums.Where(a => a.AlbumName == AlbumName && a.Year == year)
                .ToListAsync();
            if (albums == null)
            {
                return null;
            }

            return albums;
        }

        public async Task<IActionResult?> PutAlbum(int year, Album album)
        {
            if (year != album.Year)
            {
                return null;
            }

            _context.Entry(album).State = EntityState.Modified;

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

        public async Task<ActionResult<Album>?> PostAlbum(Album album)
        {
            _context.Albums.Add(album);
            await _context.SaveChangesAsync();

            return null;
        }

        
        public async Task<ActionResult<Album>?> DeleteAlbum(int id)
        {
            var album = await _context.Albums.FindAsync(id);
            if (album == null)
            {
                return null;
            }

            _context.Albums.Remove(album);
            await _context.SaveChangesAsync();

            return null;
        }
    }   
}