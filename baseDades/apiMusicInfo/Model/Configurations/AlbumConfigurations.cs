using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using apiMusicInfo.Models;


namespace apiMusicInfo.Models.Configurations
{
    public class AlbumConfigurations : IEntityTypeConfiguration<Album>
    {
        public void Configure(EntityTypeBuilder<Album> builder)
        {
            builder.HasKey(sa => new { sa.AlbumName, sa.Year, sa.SongUID });

            builder
            .HasOne(a => a.Song)
            .WithMany(s => s.Albums)
            .HasForeignKey(a => new { a.SongUID});
        }
    }
}