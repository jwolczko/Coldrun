using Coldrun.BuildingBlocks.Application.Messaging;

namespace Coldrun.Modules.TruckManagement.Application.Trucks.Commands.CreateTruck;

public sealed record CreateTruckCommand(
    string Code,
    string Name,
    string Status,
    string? Description
) : ICommand<CreateTruckResult>;
