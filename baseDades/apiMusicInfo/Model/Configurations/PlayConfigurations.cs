using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace apiMusicInfo.Models.Configurations
{
    public class PlayConfigurations : IEntityTypeConfiguration<Play>
    {
        public void Configure(EntityTypeBuilder<Play> builder)
        {
             builder.HasKey(p => new { p.SongUID, p.MusicianName, p.InstrumentName, p.Bandname});

            
            builder.HasOne(p => p.Song)
                .WithMany(s => s.Plays)
                .HasForeignKey(p => p.SongUID)
                .HasPrincipalKey(s => s.UID);

            builder.HasOne(p => p.Musician)
                .WithMany(m => m.Plays)
                .HasForeignKey(p => p.MusicianName)
                .HasPrincipalKey(m => m.Name);

            builder.HasOne(p => p.Band)
                .WithMany(b => b.Plays)
                .HasForeignKey(p => p.Bandname)
                .HasPrincipalKey(b => b.Name);
        }
    }
}
