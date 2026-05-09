using Coldrun.BuildingBlocks.Application.Messaging;
using Coldrun.Modules.TruckManagement.Application.Trucks.Dtos;
using Coldrun.Modules.TruckManagement.Domain.Trucks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coldrun.Modules.TruckManagement.Application.Trucks.Queries.GetTruckStatuses;

public sealed class GetTruckStatusesQueryHandler
    : IQueryHandler<GetTruckStatusesQuery, IReadOnlyCollection<TruckStatusMetadataDto>>
{
    public Task<IReadOnlyCollection<TruckStatusMetadataDto>> HandleAsync(
        GetTruckStatusesQuery query,
        CancellationToken cancellationToken = default)
    {
        var statuses = TruckStatus.GetAll()
            .Select(status =>
            {
                var allowedTransitions = TruckStatusTransitionPolicy
                    .GetAllowedTransitions(status)
                    .Select(x => x.Value)
                    .ToArray();

                return new TruckStatusMetadataDto(
                    status.Value,
                    allowedTransitions);
            })
            .ToArray();

        return Task.FromResult<IReadOnlyCollection<TruckStatusMetadataDto>>(statuses);
    }
}
