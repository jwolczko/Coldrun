using Coldrun.BuildingBlocks.Application.Messaging;

namespace Coldrun.Modules.TruckManagement.Application.Trucks.Commands.DeleteTruck;

public sealed record DeleteTruckCommand(
    string Code
) : ICommand;
