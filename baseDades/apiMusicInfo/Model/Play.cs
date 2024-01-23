using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace apiMusicInfo.Models
{
    public class Play
    {
        public String? Bandname { get; set; }
        public Band? Band { get; set; }

        public String? MusicianName { get; set; }
        public Musician? Musician { get; set; }

        public String? InstrumentName { get; set; }
        public Instrument? Instrument { get; set; }

        public Guid SongUID { get; set; }
        public Song? Song { get; set; }
    }
}