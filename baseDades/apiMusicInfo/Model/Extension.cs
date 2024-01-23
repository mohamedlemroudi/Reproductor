using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace apiMusicInfo.Models
{
    public class Extension
    {    
        public string Name { get; set; }= null!;
        public ICollection<Song>? Songs { get; set; }
    }
}