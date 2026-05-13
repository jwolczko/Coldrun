using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coldrun.Modules.TruckManagement.Infrastructure;

public sealed class TruckStatusConfiguration : IEntityTypeConfiguration<TruckStatusEntity>
{
    public void Configure(EntityTypeBuilder<TruckStatusEntity> builder)
    {
        builder.ToTable("TruckStatuses");
        builder.HasKey(x => x.Id)
            .HasName("PK_TruckStatuses");

        builder.Property(x => x.Code).HasMaxLength(32);
        builder.Property(x => x.Name).HasMaxLength(64);

        builder.HasIndex(x => x.Code)
            .IsUnique()
            .HasDatabaseName("UQ_TruckStatuses_Code");

        builder.HasIndex(x => x.Name)
            .IsUnique()
            .HasDatabaseName("UQ_TruckStatuses_Name");
    }
}
