namespace Coldrun.Modules.TruckManagement.Application.Trucks.Dtos;

public sealed record TruckListItemDto(
    string Code,
    string Name,
    string Status,
    string? Description);