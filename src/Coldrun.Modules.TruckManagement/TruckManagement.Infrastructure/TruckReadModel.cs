using Coldrun.Modules.TruckManagement.Application.Trucks.Ports;
using Coldrun.Modules.TruckManagement.Application.Trucks.Queries;
using Coldrun.Modules.TruckManagement.Application.Trucks.Queries.SearchTrucks;

namespace Coldrun.Modules.TruckManagement.Infrastructure;

public sealed class TruckReadModel : ITruckReadModel
{
    public Task<TruckDetailsProjection?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<TruckDetailsProjection?>(null);
    }

    public Task<TruckStatusProjection?> GetStatusByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<TruckStatusProjection?>(null);
    }

    public Task<PagedResult<TruckListItemProjection>> SearchAsync(
        SearchTrucksCriteria criteria,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new PagedResult<TruckListItemProjection>([], 1, 10, 0));
    }
}
