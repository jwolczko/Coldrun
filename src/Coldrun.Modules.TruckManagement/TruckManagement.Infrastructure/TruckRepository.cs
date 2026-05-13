using Coldrun.Modules.TruckManagement.Application.Trucks.Ports;
using Coldrun.Modules.TruckManagement.Domain.Trucks;
using Microsoft.EntityFrameworkCore;

namespace Coldrun.Modules.TruckManagement.Infrastructure;

public sealed class TruckRepository : ITruckRepository
{
    private readonly TruckManagementDbContext _dbContext;

    public TruckRepository(TruckManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Truck?> GetByCodeAsync(TruckCode code, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Trucks
            .AsNoTracking()
            .Include(x => x.Status)
            .FirstOrDefaultAsync(
                x => x.Code == code.Value,
                cancellationToken);

        return entity is null
            ? null
            : MapToDomain(entity);
    }

    public Task<bool> ExistsByCodeAsync(TruckCode code, CancellationToken cancellationToken)
    {
        return _dbContext.Trucks.AnyAsync(
            x => x.Code == code.Value,
            cancellationToken);
    }

    public async Task AddAsync(Truck truck, CancellationToken cancellationToken)
    {
        var statusId = await GetStatusIdAsync(
            truck.Status,
            cancellationToken);

        var entity = new TruckEntity
        {
            Code = truck.Code.Value,
            Name = truck.Name.Value,
            StatusId = statusId,
            Description = truck.Description?.Value
        };

        await _dbContext.Trucks.AddAsync(
            entity,
            cancellationToken);
    }

    public async Task UpdateAsync(Truck truck, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Trucks.FirstOrDefaultAsync(
            x => x.Code == truck.Code.Value,
            cancellationToken);

        if (entity is null)
        {
            return;
        }

        entity.Name = truck.Name.Value;
        entity.StatusId = await GetStatusIdAsync(
            truck.Status,
            cancellationToken);
        entity.Description = truck.Description?.Value;
    }

    public void Remove(Truck truck)
    {
        _dbContext.Trucks.Remove(new TruckEntity
        {
            Code = truck.Code.Value
        });
    }

    private async Task<short> GetStatusIdAsync(
        TruckStatus status,
        CancellationToken cancellationToken)
    {
        return await _dbContext.TruckStatuses
            .Where(x => x.Name == status.Value)
            .Select(x => x.Id)
            .SingleAsync(cancellationToken);
    }

    private static Truck MapToDomain(TruckEntity entity)
    {
        return Truck.Create(
            TruckCode.Create(entity.Code),
            TruckName.Create(entity.Name),
            TruckStatus.From(entity.Status.Name),
            TruckDescription.CreateOptional(entity.Description));
    }
}
