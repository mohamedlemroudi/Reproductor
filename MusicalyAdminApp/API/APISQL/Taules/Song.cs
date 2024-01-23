using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalyAdminApp.API.APISQL.Taules
{
    public class Song
    {
        [Key]
        public Guid UID { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = null!;
        public string? Language { get; set; }
        public int? Duration { get; set; }

        public Guid? VersionOriginalId { get; set; }
        public Song? OriginalSong { get; set; }
        public Play? PlayObj { get; set; }
        public ICollection<Song> Songs { get; set; } = new List<Song>();
        public ICollection<Play> Plays { get; set; } = new List<Play>();
        public ICollection<Extension> Extensions { get; set; } = new List<Extension>();
        public ICollection<PlayList> PlayLists { get; set; }

        public ICollection<Album> Albums { get; set; } = new List<Album>();

    }
}
