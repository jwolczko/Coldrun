using Coldrun.Modules.TruckManagement.Domain.Trucks;

namespace Coldrun.Modules.TruckManagement.Application.Ports;

public interface ITruckRepository
{
    Task<Truck?> GetByCodeAsync(TruckCode code, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(TruckCode code, CancellationToken cancellationToken);
    Task AddAsync(Truck truck, CancellationToken cancellationToken);
    void Remove(Truck truck);
}
