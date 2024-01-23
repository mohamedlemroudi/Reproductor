using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;

namespace apiMusicInfo.Models
{
    public class Playlist
    {
        public string Dispositiu { get; set;}= null!;

        public string PlaylistName { get; set;}= null!;

        public DateTime? CreationDate { get; set;}

        public ICollection<Song>? Songs { get; set; }
    }
}