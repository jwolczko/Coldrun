namespace Coldrun.Modules.TruckManagement.Application.Queries;

public sealed record TruckDetailsDto(
    string Code,
    string Name,
    string Status,
    string? Description);

public sealed record TruckListItemDto(
    string Code,
    string Name,
    string Status);

public sealed record PagedResult<T>(
    IReadOnlyCollection<T> Items,
    int PageNumber,
    int PageSize,
    int TotalCount);
