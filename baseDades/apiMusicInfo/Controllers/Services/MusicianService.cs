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
    public class MusicianService
    {
        private readonly DataContext _context;

        public MusicianService(DataContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<Musician>>> GetMusicians()
        {
            return await _context.Musician.ToListAsync();
        }

        public async Task<ActionResult<Musician>?> GetMusician(string name)
        {
            var musician = await _context.Musician.FindAsync(name);

            if (musician == null)
            {
                return null;
            }

            return musician;
        }

        public async Task<IActionResult?> PutMusician(string name, Musician musician)
        {
            if (name != musician.Name)
            {
                return null;
            }

            _context.Entry(musician).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return null;
            }

            return null;
        }

        public async Task<ActionResult<Musician>?> PostMusician(Musician musician)
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
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return musician;
        }

        public async Task<ServiceResult> AddMusicianBand(string nameMusician, string nameBand)
        {
            var musician = await _context.Musician
                                          .Include(m => m.Bands)
                                          .FirstOrDefaultAsync(m => m.Name == nameMusician);

            if (musician == null)
            {
                return ServiceResult.Failure($"Musician with name '{nameMusician}' not found.");
            }

            var band = await _context.Band.FirstOrDefaultAsync(b => b.Name == nameBand);

            if (musician.Bands != null && musician.Bands.Any(b => b.Name == nameBand))
            {
                return ServiceResult.Failure($"Musician '{nameMusician}' is already associated with band '{nameBand}'.");
            }
            if (musician.Bands == null)
            {
                musician.Bands = new List<Band>();
            }
            if (band == null)
            {
                return ServiceResult.Failure($"Band with name '{nameBand}' not found.");
            }

            musician.Bands.Add(band);

            try
            {
                await _context.SaveChangesAsync();
                return ServiceResult.Success($"Added musician '{nameMusician}' to band '{nameBand}' successfully.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MusicianExists(nameMusician))
                {
                    return ServiceResult.Failure($"Musician with name '{nameMusician}' not found.");
                }
                else
                {
                    return ServiceResult.Failure("Concurrency exception occurred.");
                }
            }
        }


        public async Task<ActionResult<Musician>?> DeleteMusician(string name)
        {
            var musician = await _context.Musician.FindAsync(name);
            if (musician == null)
            {
                return null;
            }

            _context.Musician.Remove(musician);
            await _context.SaveChangesAsync();

            return musician;
        }

        private bool MusicianExists(string name)
        {
            return _context.Musician.Any(e => e.Name == name);
        }

    }
}