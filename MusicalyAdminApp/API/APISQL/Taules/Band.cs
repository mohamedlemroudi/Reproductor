using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalyAdminApp.API.APISQL.Taules
{
    public class Band
    {
        [Key]
        [MaxLength(15)]
        public string Name { get; set; } = null!;

        [MaxLength(15)]
        public string? Origin { get; set; }

        [MaxLength(15)]
        public string? Genre { get; set; }

        public ICollection<Play> plays { get; set; } = new List<Play>();

        public ICollection<Musician> Musicians { get; set; } = new List<Musician>();
    }
}
