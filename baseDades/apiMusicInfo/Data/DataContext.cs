using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using apiMusicInfo.Models;
using apiMusicInfo.Models.Configurations;
using Microsoft.EntityFrameworkCore;

namespace apiMusicInfo.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SongConfigurations());
            modelBuilder.ApplyConfiguration(new MusicianConfigurations());
            modelBuilder.ApplyConfiguration(new PlayConfigurations());
            modelBuilder.ApplyConfiguration(new ExtensionConfigurations());
            modelBuilder.ApplyConfiguration(new PlaylistConfigurations());
            modelBuilder.ApplyConfiguration(new InstrumentConfigurations());
            modelBuilder.ApplyConfiguration(new AlbumConfigurations());
        }

        public DbSet<Musician> Musician { get; set; } = null!;
        public DbSet<Band> Band { get; set; } = null!;
        public DbSet<Song> Songs { get; set; }
        public DbSet<Extension> Extensions { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Play> Plays { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Instrument> Instruments {get; set;}
    }
}