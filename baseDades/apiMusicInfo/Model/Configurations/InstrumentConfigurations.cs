using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace apiMusicInfo.Models.Configurations
{
    public class InstrumentConfigurations : IEntityTypeConfiguration<Instrument>
    {
        public void Configure(EntityTypeBuilder<Instrument> builder)
        {
            builder.HasKey(i => i.Name);

            builder.HasMany(i => i.Plays)
                .WithOne(p => p.Instrument)
                .HasForeignKey(p => p.InstrumentName)
                .HasPrincipalKey(i => i.Name);
                
        }
    }
}
