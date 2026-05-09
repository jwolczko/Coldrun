using Coldrun.Modules.TruckManagement.Application.Trucks.Queries;
using Coldrun.Modules.TruckManagement.Application.Trucks.Queries.SearchTrucks;

namespace Coldrun.Modules.TruckManagement.Application.Trucks.Ports;

public interface ITruckReadModel
{
    Task<TruckDetailsProjection?> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default);

    Task<TruckStatusProjection?> GetStatusByCodeAsync(
        string code,
        CancellationToken cancellationToken = default);

    Task<PagedResult<TruckListItemProjection>> SearchAsync(
        SearchTrucksCriteria criteria,
        CancellationToken cancellationToken = default);
}

public sealed record TruckDetailsProjection(
    string Code,
    string Name,
    string Status,
    string? Description);

public sealed record TruckStatusProjection(
    string Code,
    string Status);

public sealed record TruckListItemProjection(
    string Code,
    string Name,
    string Status,
    string? Description);