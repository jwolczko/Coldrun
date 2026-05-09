using Coldrun.BuildingBlocks.Application.Messaging;
using Coldrun.Modules.TruckManagement.Application.Trucks.Dtos;

namespace Coldrun.Modules.TruckManagement.Application.Trucks.Queries.GetTruckStatus;

public sealed record GetTruckStatusQuery(
    string Code
) : IQuery<TruckStatusDto?>;
