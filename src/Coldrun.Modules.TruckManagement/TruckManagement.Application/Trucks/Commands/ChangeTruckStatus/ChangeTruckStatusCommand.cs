using Coldrun.BuildingBlocks.Application.Messaging;

namespace Coldrun.Modules.TruckManagement.Application.Trucks.Commands.ChangeTruckStatus;

public sealed record ChangeTruckStatusCommand(
    string Code,
    string Status
) : ICommand<ChangeTruckStatusResult>;
