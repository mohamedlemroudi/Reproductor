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
    public class InstrumentController : ControllerBase
    {
        private readonly DataContext _context;

        private readonly InstrumentService _InstrumentService;

        public InstrumentController(DataContext context)
        {
            _context = context;
            _InstrumentService = new InstrumentService(context);
        }

        // GET: api/Instrument
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Instrument>>> GetInstruments()
        {
                return await _InstrumentService.GetInstruments();
        }

        // GET: api/Instrument/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Instrument>> GetInstrument(string id)
        {
            var instrument = await _InstrumentService.GetInstrument(id);

            if (instrument == null)
            {
                return NotFound();
            }

            return Ok(instrument);
        }

        // PUT: api/Instrument/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInstrument(string name, Instrument instrument)
        {
            if (name != instrument.Name)
            {
                return BadRequest();
            }

            var result = await _InstrumentService.PutInstrument(name, instrument);

            return Ok(result);
        }

        // POST: api/Instrument
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Instrument>> PostInstrument(Instrument instrument)
        {
            var result = await _InstrumentService.PostInstrument(instrument);

            return Ok(result);
        }

        // DELETE: api/Instrument/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInstrument(string id)
        {
            var result = await _InstrumentService.DeleteInstrument(id);

            return Ok(result);
        }
    }
}
