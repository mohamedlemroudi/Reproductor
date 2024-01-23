using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalyAdminApp.API.APISQL.Taules
{  
    public class Musician
    {
        [Key]
        [MaxLength(15)]
        public string Name { get; set; } = null!;

        [Range(0, 99, ErrorMessage = "La edad debe estar entre 0 y 99.")]
        public int? Age { get; set; }

        public ICollection<Play> Plays { get; set; } = new List<Play>();
        public ICollection<Band> Bands { get; set; } = new List<Band>();
    }
}
