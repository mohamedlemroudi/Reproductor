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
    public class BandService
    {
        private readonly DataContext _context;

        public BandService(DataContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<Band>>> GetBand()
        {
            return await _context.Band.ToListAsync();
        }

        public async Task<ActionResult<Band>?> GetBand(string name)
        {
            //var band = await _context.Band.FindAsync(id);
            var band = await _context.Band.Include(b => b.Musicians).FirstOrDefaultAsync(b => b.Name == name);

            if (band == null)
            {
                return null;
            }

            return band;
        }

        public async Task<IActionResult?> PutBand(string id, Band band)
        {
            if (id != band.Name)
            {
                return null;
            }

            _context.Entry(band).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                    throw;
            }

            return null;
        }

        public async Task<ActionResult<Band>?> PostBand(Band band)
        {
            _context.Band.Add(band);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BandExists(band.Name))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return null;
        }

        public async Task<ActionResult<Band>?> DeleteBand(string id)
        {
            var band = await _context.Band.FindAsync(id);
            if (band == null)
            {
                return null;
            }

            _context.Band.Remove(band);
            await _context.SaveChangesAsync();

            return null;
        }

        private bool BandExists(string id)
        {
            return _context.Band.Any(e => e.Name == id);
        }
    }
}