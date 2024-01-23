using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using apiMusicInfo.Models; 

namespace apiMusicInfo.Models
{
    public class Album
    {
        [Key]
        public string? AlbumName { get; set; }

        [Key]
        public int Year { get; set; } 
        [Key]
        public Guid SongUID { get; set; } 
        public Song? Song { get; set; }
    }
}