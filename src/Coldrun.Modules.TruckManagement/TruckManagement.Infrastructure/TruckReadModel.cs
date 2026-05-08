using Coldrun.Modules.TruckManagement.Application.Ports;
using Coldrun.Modules.TruckManagement.Application.Queries;

namespace Coldrun.Modules.TruckManagement.Infrastructure;

public sealed class TruckReadModel : ITruckReadModel
{
    public Task<TruckDetailsDto?> GetByCodeAsync(string code, CancellationToken cancellationToken)
    {
        return Task.FromResult<TruckDetailsDto?>(null);
    }

    public Task<PagedResult<TruckListItemDto>> SearchAsync(
        SearchTrucksQuery query,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(new PagedResult<TruckListItemDto>([], 1, 10, 0));
    }
}
