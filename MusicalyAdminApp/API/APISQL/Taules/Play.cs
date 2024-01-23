using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalyAdminApp.API.APISQL.Taules
{
    public class Play
    {
        public Guid UIDSong { get; set; }
        public ICollection<Song> Songs { get; set; } = new List<Song>();

        public string? NameMusician { get; set; }
        public ICollection<Musician> Musicians { get; set; } = new List<Musician>();

        public string? NameBand { get; set; }
        public ICollection<Band> Bands { get; set; } = new List<Band>();

        public string? NameInstrument { get; set; }

        public ICollection<Instrument> Instruments { get; set; } = new List<Instrument>();

       
    }
}
