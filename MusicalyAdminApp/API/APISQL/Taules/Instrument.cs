using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalyAdminApp.API.APISQL.Taules
{
    public class Instrument
    {
        [Key]
        public string Name { get; set; } = null!;

        public string Type { get; set; } = null!;

        public ICollection<Play> Plays { get; set; } = new List<Play>();
    }
}
