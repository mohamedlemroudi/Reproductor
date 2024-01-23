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
    public class ExtensionController : ControllerBase
    {
        private readonly DataContext _context;
         private readonly ExtensionService _ExtensionService;

        public ExtensionController(DataContext context)
        {
            _context = context;
            _ExtensionService = new ExtensionService(context);
        }

        // GET: api/Extension
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Extension>>> GetExtensions()
        {
                return await _ExtensionService.GetExtensions();
        }

        // GET: api/Extension/5
        [HttpGet("{name}")]
        public async Task<ActionResult<Extension>> GetExtension(string name)
        {
            var extension = await _ExtensionService.GetExtension(name);

            if (extension == null)
            {
                return NotFound();
            }

            return Ok(extension);
        }

        // PUT: api/Extension/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{name}")]
        public async Task<IActionResult> PutExtension(string name, Extension extension)
        {
            if (name != extension.Name)
            {
                return BadRequest();
            }

            var result = await _ExtensionService.PutExtension(name, extension);

            return Ok(result);
        }

        // POST: api/Extension
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Extension>> PostExtension(Extension extension)
        {
            var result = await _ExtensionService.PostExtension(extension);

            return Ok(result);
        }

        // DELETE: api/Extension/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExtension(string id)
        {
            var result = await _ExtensionService.DeleteExtension(id);

            return Ok(result);
        }
    }
}
