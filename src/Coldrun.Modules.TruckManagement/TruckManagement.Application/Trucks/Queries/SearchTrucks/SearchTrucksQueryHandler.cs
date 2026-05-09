using Coldrun.BuildingBlocks.Application.Messaging;
using Coldrun.Modules.TruckManagement.Application.Trucks.Exceptions;
using Coldrun.Modules.TruckManagement.Application.Trucks.Ports;
using Coldrun.Modules.TruckManagement.Domain.Trucks;

namespace Coldrun.Modules.TruckManagement.Application.Trucks.Queries.SearchTrucks;

public sealed class SearchTrucksQueryHandler
    : IQueryHandler<SearchTrucksQuery, PagedResult<TruckListItemDto>>
{
    private const int MaxPageSize = 100;

    private static readonly IReadOnlyDictionary<string, string> AllowedSortFields =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["code"] = "Code",
            ["name"] = "Name",
            ["status"] = "Status",
            ["description"] = "Description"
        };

    private readonly ITruckReadModel _truckReadModel;

    public SearchTrucksQueryHandler(ITruckReadModel truckReadModel)
    {
        _truckReadModel = truckReadModel;
    }

    public async Task<PagedResult<TruckListItemDto>> HandleAsync(
        SearchTrucksQuery query,
        CancellationToken cancellationToken = default)
    {
        ValidatePagination(
            query.PageNumber,
            query.PageSize);

        var normalizedStatus = NormalizeStatus(query.Status);

        var criteria = new SearchTrucksCriteria(
            Code: NormalizeNullable(query.Code),
            CodeContains: NormalizeNullable(query.CodeContains),
            NameContains: NormalizeNullable(query.NameContains),
            Status: normalizedStatus,
            DescriptionContains: NormalizeNullable(query.DescriptionContains),
            Sort: ParseSort(query.Sort),
            PageNumber: query.PageNumber,
            PageSize: query.PageSize);

        var result = await _truckReadModel.SearchAsync(
            criteria,
            cancellationToken);

        var items = result.Items
            .Select(x => new TruckListItemDto(
                x.Code,
                x.Name,
                x.Status,
                x.Description))
            .ToArray();

        return new PagedResult<TruckListItemDto>(
            items,
            result.PageNumber,
            result.PageSize,
            result.TotalElements);
    }

    private static string? NormalizeStatus(string? status)
    {
        if (string.IsNullOrWhiteSpace(status))
        {
            return null;
        }

        return TruckStatus.From(status).Value;
    }

    private static string? NormalizeNullable(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }

    private static void ValidatePagination(
        int pageNumber,
        int pageSize)
    {
        if (pageNumber < 1)
        {
            throw new InvalidPaginationException(
                "Page number must be greater than or equal to 1.");
        }

        if (pageSize < 1)
        {
            throw new InvalidPaginationException(
                "Page size must be greater than or equal to 1.");
        }

        if (pageSize > MaxPageSize)
        {
            throw new InvalidPaginationException(
                $"Page size cannot be greater than {MaxPageSize}.");
        }
    }

    private static IReadOnlyCollection<TruckSortField> ParseSort(string? sort)
    {
        if (string.IsNullOrWhiteSpace(sort))
        {
            return new[]
            {
                new TruckSortField(
                    "Code",
                    TruckSortDirection.Ascending)
            };
        }

        var result = new List<TruckSortField>();

        var fields = sort.Split(
            ',',
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        foreach (var rawField in fields)
        {
            var direction = rawField.StartsWith('-')
                ? TruckSortDirection.Descending
                : TruckSortDirection.Ascending;

            var fieldName = rawField.StartsWith('-')
                ? rawField[1..]
                : rawField;

            if (string.IsNullOrWhiteSpace(fieldName))
            {
                throw new InvalidTruckSortFieldException(rawField);
            }

            if (!AllowedSortFields.TryGetValue(fieldName, out var normalizedField))
            {
                throw new InvalidTruckSortFieldException(fieldName);
            }

            result.Add(new TruckSortField(
                normalizedField,
                direction));
        }

        return result;
    }
}
