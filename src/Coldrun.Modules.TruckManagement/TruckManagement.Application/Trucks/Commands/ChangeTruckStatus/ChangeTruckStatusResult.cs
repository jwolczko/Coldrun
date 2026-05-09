namespace Coldrun.Modules.TruckManagement.Application.Trucks.Commands.ChangeTruckStatus;

public sealed record ChangeTruckStatusResult(
    string Code,
    string Status,
    IReadOnlyCollection<string> AllowedStatusTransitions);