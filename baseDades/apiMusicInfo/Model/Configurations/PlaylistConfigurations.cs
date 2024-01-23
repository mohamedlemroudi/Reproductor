using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace apiMusicInfo.Models.Configurations
{
    public class PlaylistConfigurations : IEntityTypeConfiguration<Playlist>
    {
        public void Configure(EntityTypeBuilder<Playlist> builder)
        {
            builder.HasKey(p => new {p.Dispositiu, p.PlaylistName});
            builder.HasIndex(p => p.Dispositiu);
        }
    }
}