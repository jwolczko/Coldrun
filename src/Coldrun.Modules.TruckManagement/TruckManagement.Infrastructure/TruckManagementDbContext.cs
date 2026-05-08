using Microsoft.EntityFrameworkCore;

namespace Coldrun.Modules.TruckManagement.Infrastructure;

public sealed class TruckManagementDbContext : DbContext
{
    public TruckManagementDbContext(DbContextOptions<TruckManagementDbContext> options)
        : base(options)
    {
    }

    public DbSet<TruckEntity> Trucks => Set<TruckEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("truck_management");

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(TruckManagementDbContext).Assembly);
    }
}
