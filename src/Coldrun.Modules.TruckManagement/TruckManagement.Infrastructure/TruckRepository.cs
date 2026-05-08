using Coldrun.Modules.TruckManagement.Application.Ports;
using Coldrun.Modules.TruckManagement.Domain.Trucks;

namespace Coldrun.Modules.TruckManagement.Infrastructure;

public sealed class TruckRepository : ITruckRepository
{
    public Task<Truck?> GetByCodeAsync(TruckCode code, CancellationToken cancellationToken)
    {
        return Task.FromResult<Truck?>(null);
    }

    public Task<bool> ExistsAsync(TruckCode code, CancellationToken cancellationToken)
    {
        return Task.FromResult(false);
    }

    public Task AddAsync(Truck truck, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public void Remove(Truck truck)
    {
    }
}
