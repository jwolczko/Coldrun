using Coldrun.BuildingBlocks.Application.Messaging;

namespace Coldrun.Modules.TruckManagement.Application.Trucks.Queries.SearchTrucks;

public sealed record SearchTrucksQuery(
    string? Code,
    string? CodeContains,
    string? NameContains,
    string? Status,
    string? DescriptionContains,
    string? Sort,
    int PageNumber,
    int PageSize
) : IQuery<PagedResult<TruckListItemDto>>;
