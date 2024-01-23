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
    public class ExtensionService
    {
        private readonly DataContext _context;

        public ExtensionService(DataContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<Extension>>> GetExtensions()
        {
            return await _context.Extensions.ToListAsync();
        }

        public async Task<ActionResult<Extension>?> GetExtension(string name)
        {
            var extension = await _context.Extensions.FindAsync(name);

            if (extension == null)
            {
                return null;
            }

            return extension;
        }

        public async Task<IActionResult?> PutExtension(string name, Extension extension)
        {
            if (name != extension.Name)
            {
                return null;
            }

            _context.Entry(extension).State = EntityState.Modified;

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

        public async Task<ActionResult<Extension>?> PostExtension(Extension extension)
        {
            _context.Extensions.Add(extension);
            await _context.SaveChangesAsync();

            return null;
        }

        public async Task<IActionResult?> DeleteExtension(string name)
        {
            var extension = await _context.Extensions.FindAsync(name);
            if (extension == null)
            {
                return null;
            }

            _context.Extensions.Remove(extension);
            await _context.SaveChangesAsync();

            return null;
        }

    }
}