namespace Coldrun.Modules.TruckManagement.Application.Trucks.Queries.SearchTrucks;

public sealed record SearchTrucksCriteria(
    string? Code,
    string? CodeContains,
    string? NameContains,
    string? Status,
    string? DescriptionContains,
    IReadOnlyCollection<TruckSortField> Sort,
    int PageNumber,
    int PageSize);
