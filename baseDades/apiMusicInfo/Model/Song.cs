using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace apiMusicInfo.Models
{
    public class Song
    {
        [Key]
        public Guid UID { get; set; } = Guid.NewGuid();
        
        public string Title { get; set; } = null!;
        
        public string? Language { get; set; }
        public int? Duration { get; set; }

        // Reflexive relationship
        public Guid? VersionOriginalId { get; set; }
        public ICollection<Song> DerivedVersions { get; set; } = new List<Song>();
        public Song? OriginalSong { get; set; }

        // Relationship with Play
        public ICollection<Play> Plays { get; set; } = new List<Play>();

        // Relationship with Extension
        public ICollection<Extension>? Extensions { get; set; }

        // Relationship with Playlist
        public ICollection<Playlist>? Playlists { get; set; }

        // Self-referencing relationship with SongAlbum
        public ICollection<Album> Albums { get; set; } = new List<Album>();
    }
}
