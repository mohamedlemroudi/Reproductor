using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace apiMusicInfo.Models
{
    public class Instrument
    {
        
        [Key]
        public string? Name { get; set; }
        public string? Type { get; set; }
        public ICollection<Play> Plays { get; set; } = new List<Play>();
    }
}
