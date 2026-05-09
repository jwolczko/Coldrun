namespace Coldrun.Modules.TruckManagement.Application.Trucks.Commands.CreateTruck;

public sealed record CreateTruckResult(
    string Code,
    string Name,
    string Status,
    string? Description,
    IReadOnlyCollection<string> AllowedStatusTransitions);
