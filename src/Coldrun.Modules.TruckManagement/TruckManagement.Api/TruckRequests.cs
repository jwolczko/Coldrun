namespace Coldrun.Modules.TruckManagement.Api;

public sealed record CreateTruckRequest(
    string Code,
    string Name,
    string? Description);

public sealed record UpdateTruckDetailsRequest(
    string Name,
    string? Description);

public sealed record ChangeTruckStatusRequest(
    string Status);
