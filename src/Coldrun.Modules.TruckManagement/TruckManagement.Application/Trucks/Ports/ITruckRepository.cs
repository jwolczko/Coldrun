using Coldrun.Modules.TruckManagement.Domain.Trucks;

namespace Coldrun.Modules.TruckManagement.Application.Trucks.Ports;

public interface ITruckRepository
{
    Task<Truck?> GetByCodeAsync(
        TruckCode code,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByCodeAsync(
        TruckCode code,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Truck truck,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        Truck truck,
        CancellationToken cancellationToken = default);

    void Remove(Truck truck);
}
