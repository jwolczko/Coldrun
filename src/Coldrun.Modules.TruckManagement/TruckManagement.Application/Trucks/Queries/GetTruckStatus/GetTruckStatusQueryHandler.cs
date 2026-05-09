using Coldrun.BuildingBlocks.Application.Messaging;
using Coldrun.Modules.TruckManagement.Application.Trucks.Dtos;
using Coldrun.Modules.TruckManagement.Application.Trucks.Ports;
using Coldrun.Modules.TruckManagement.Domain.Trucks;

namespace Coldrun.Modules.TruckManagement.Application.Trucks.Queries.GetTruckStatus;

public sealed class GetTruckStatusQueryHandler
    : IQueryHandler<GetTruckStatusQuery, TruckStatusDto?>
{
    private readonly ITruckReadModel _truckReadModel;

    public GetTruckStatusQueryHandler(ITruckReadModel truckReadModel)
    {
        _truckReadModel = truckReadModel;
    }

    public async Task<TruckStatusDto?> HandleAsync(
        GetTruckStatusQuery query,
        CancellationToken cancellationToken = default)
    {
        var code = TruckCode.Create(query.Code);

        var projection = await _truckReadModel.GetStatusByCodeAsync(
            code.Value,
            cancellationToken);

        if (projection is null)
        {
            return null;
        }

        var status = TruckStatus.From(projection.Status);

        var allowedTransitions = TruckStatusTransitionPolicy
            .GetAllowedTransitions(status)
            .Select(x => x.Value)
            .ToArray();

        return new TruckStatusDto(
            projection.Code,
            projection.Status,
            allowedTransitions);
    }
}
