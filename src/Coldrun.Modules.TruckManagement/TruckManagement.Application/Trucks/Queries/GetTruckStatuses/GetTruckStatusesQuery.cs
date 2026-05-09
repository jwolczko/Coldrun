using Coldrun.BuildingBlocks.Application.Messaging;
using Coldrun.Modules.TruckManagement.Application.Trucks.Dtos;

namespace Coldrun.Modules.TruckManagement.Application.Trucks.Queries.GetTruckStatuses;

public sealed record GetTruckStatusesQuery
    : IQuery<IReadOnlyCollection<TruckStatusMetadataDto>>;
