using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using apiMusicInfo.Models;  


namespace apiMusicInfo.Models.Configurations
{
    public class SongConfigurations : IEntityTypeConfiguration<Song>
    {
        public void Configure(EntityTypeBuilder<Song> builder)
        {
            builder.HasKey(s => new { s.UID});
            builder.HasIndex(s => s.UID);
        }
    }
}