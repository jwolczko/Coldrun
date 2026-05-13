using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coldrun.Modules.TruckManagement.Infrastructure;

public sealed class TruckConfiguration : IEntityTypeConfiguration<TruckEntity>
{
    public void Configure(EntityTypeBuilder<TruckEntity> builder)
    {
        builder.ToTable("Trucks");
        builder.HasKey(x => x.Code)
            .HasName("PK_Trucks");

        builder.Property(x => x.Code).HasMaxLength(64);
        builder.Property(x => x.Name).HasMaxLength(256);
        builder.Property(x => x.Description).HasMaxLength(2000);

        builder
            .HasOne(x => x.Status)
            .WithMany(x => x.Trucks)
            .HasForeignKey(x => x.StatusId)
            .HasConstraintName("FK_Trucks_TruckStatuses_StatusId");

        builder.HasIndex(x => x.StatusId)
            .HasDatabaseName("IX_Trucks_StatusId");

        builder.HasIndex(x => x.Name)
            .HasDatabaseName("IX_Trucks_Name");

        builder.HasIndex(x => x.Description)
            .HasDatabaseName("IX_Trucks_Description");
    }
}
