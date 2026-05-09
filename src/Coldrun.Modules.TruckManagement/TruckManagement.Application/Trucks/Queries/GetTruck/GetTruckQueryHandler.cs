using Coldrun.BuildingBlocks.Application.Messaging;
using Coldrun.Modules.TruckManagement.Application.Trucks.Ports;
using Coldrun.Modules.TruckManagement.Domain.Trucks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coldrun.Modules.TruckManagement.Application.Trucks.Queries.GetTruck;

public sealed class GetTruckQueryHandler
    : IQueryHandler<GetTruckQuery, TruckDetailsDto?>
{
    private readonly ITruckReadModel _truckReadModel;

    public GetTruckQueryHandler(ITruckReadModel truckReadModel)
    {
        _truckReadModel = truckReadModel;
    }

    public async Task<TruckDetailsDto?> HandleAsync(
        GetTruckQuery query,
        CancellationToken cancellationToken = default)
    {
        var code = TruckCode.Create(query.Code);

        var projection = await _truckReadModel.GetByCodeAsync(
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

        return new TruckDetailsDto(
            projection.Code,
            projection.Name,
            projection.Status,
            projection.Description,
            allowedTransitions);
    }
}
