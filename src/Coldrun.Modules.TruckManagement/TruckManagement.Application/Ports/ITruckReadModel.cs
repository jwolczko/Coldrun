using Coldrun.Modules.TruckManagement.Application.Queries;

namespace Coldrun.Modules.TruckManagement.Application.Ports;

public interface ITruckReadModel
{
    Task<TruckDetailsDto?> GetByCodeAsync(
       string code,
       CancellationToken cancellationToken);

    Task<PagedResult<TruckListItemDto>> SearchAsync(
        SearchTrucksQuery query,
        CancellationToken cancellationToken);
}
