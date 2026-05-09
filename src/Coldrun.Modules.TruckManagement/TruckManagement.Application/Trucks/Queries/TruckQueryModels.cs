namespace Coldrun.Modules.TruckManagement.Application.Trucks.Queries;

public sealed record TruckDetailsDto(
    string Code,
    string Name,
    string Status,
    string? Description,
    IReadOnlyCollection<string> AllowedStatusTransitions);

public sealed record TruckListItemDto(
    string Code,
    string Name,
    string Status,
    string? Description);

public sealed record PagedResult<T>(
    IReadOnlyCollection<T> Items,
    int PageNumber,
    int PageSize,
    long TotalElements)
{
    public int TotalPages =>
        PageSize <= 0
            ? 0
            : (int)Math.Ceiling((double)TotalElements / PageSize);

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;
}
