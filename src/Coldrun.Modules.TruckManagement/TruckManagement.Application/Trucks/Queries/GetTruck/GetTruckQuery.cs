using Coldrun.BuildingBlocks.Application.Messaging;

namespace Coldrun.Modules.TruckManagement.Application.Trucks.Queries.GetTruck;

public sealed record GetTruckQuery(
    string Code
) : IQuery<TruckDetailsDto?>;
