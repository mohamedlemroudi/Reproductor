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
    public class InstrumentService
    {
        private readonly DataContext _context;

        public InstrumentService(DataContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<Instrument>>> GetInstruments()
        {
            return await _context.Instruments.ToListAsync();
        }

        public async Task<ActionResult<Instrument>?> GetInstrument(string name)
        {
            var instrument = await _context.Instruments.FindAsync(name);

            if (instrument == null)
            {
                return null;
            }

            return instrument;
        }

        public async Task<IActionResult?> PutInstrument(string name, Instrument instrument)
        {
            if (name != instrument.Name)
            {
                return null;
            }

            _context.Entry(instrument).State = EntityState.Modified;

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

        public async Task<ActionResult<Instrument>?> PostInstrument(Instrument instrument)
        {
            _context.Instruments.Add(instrument);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return null;
            }

            return null;
        }

        public async Task<IActionResult?> DeleteInstrument(string name)
        {
            var instrument = await _context.Instruments.FindAsync(name);
            if (instrument == null)
            {
                return null;
            }

            _context.Instruments.Remove(instrument);
            await _context.SaveChangesAsync();

            return null;
        }
        
    }
}