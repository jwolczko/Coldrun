using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coldrun.Modules.TruckManagement.Infrastructure;

public sealed class TruckConfiguration : IEntityTypeConfiguration<TruckEntity>
{
    public void Configure(EntityTypeBuilder<TruckEntity> builder)
    {
        builder.ToTable("trucks");
        builder.HasKey(x => x.Code);

        builder.Property(x => x.Code).HasMaxLength(64);
        builder.Property(x => x.Name).HasMaxLength(256);
        builder.Property(x => x.Status).HasMaxLength(64);
        builder.Property(x => x.Description).HasMaxLength(2000);
    }
}
