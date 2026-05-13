using Coldrun.Modules.TruckManagement.Application.Trucks.Ports;
using Coldrun.Modules.TruckManagement.Application.Trucks.Queries;
using Coldrun.Modules.TruckManagement.Application.Trucks.Queries.SearchTrucks;
using Microsoft.EntityFrameworkCore;

namespace Coldrun.Modules.TruckManagement.Infrastructure;

public sealed class TruckReadModel : ITruckReadModel
{
    private readonly TruckManagementDbContext _dbContext;

    public TruckReadModel(TruckManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<TruckDetailsProjection?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return _dbContext.Trucks
            .AsNoTracking()
            .Where(x => x.Code == code)
            .Select(x => new TruckDetailsProjection(
                x.Code,
                x.Name,
                x.Status.Name,
                x.Description))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<TruckStatusProjection?> GetStatusByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return _dbContext.Trucks
            .AsNoTracking()
            .Where(x => x.Code == code)
            .Select(x => new TruckStatusProjection(
                x.Code,
                x.Status.Name))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedResult<TruckListItemProjection>> SearchAsync(
        SearchTrucksCriteria criteria,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Trucks
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(criteria.Code))
        {
            query = query.Where(x => x.Code == criteria.Code);
        }

        if (!string.IsNullOrWhiteSpace(criteria.CodeContains))
        {
            query = query.Where(x => x.Code.Contains(criteria.CodeContains));
        }

        if (!string.IsNullOrWhiteSpace(criteria.NameContains))
        {
            query = query.Where(x => x.Name.Contains(criteria.NameContains));
        }

        if (!string.IsNullOrWhiteSpace(criteria.Status))
        {
            query = query.Where(x => x.Status.Name == criteria.Status);
        }

        if (!string.IsNullOrWhiteSpace(criteria.DescriptionContains))
        {
            query = query.Where(x =>
                x.Description != null &&
                x.Description.Contains(criteria.DescriptionContains));
        }

        var totalElements = await query.LongCountAsync(cancellationToken);

        query = ApplySort(
            query,
            criteria.Sort);

        var items = await query
            .Skip((criteria.PageNumber - 1) * criteria.PageSize)
            .Take(criteria.PageSize)
            .Select(x => new TruckListItemProjection(
                x.Code,
                x.Name,
                x.Status.Name,
                x.Description))
            .ToArrayAsync(cancellationToken);

        return new PagedResult<TruckListItemProjection>(
            items,
            criteria.PageNumber,
            criteria.PageSize,
            totalElements);
    }

    private static IQueryable<TruckEntity> ApplySort(
        IQueryable<TruckEntity> query,
        IReadOnlyCollection<TruckSortField> sort)
    {
        IOrderedQueryable<TruckEntity>? orderedQuery = null;

        foreach (var sortField in sort)
        {
            orderedQuery = ApplySortField(
                orderedQuery ?? query,
                sortField,
                orderedQuery is not null);
        }

        return orderedQuery ?? query.OrderBy(x => x.Code);
    }

    private static IOrderedQueryable<TruckEntity> ApplySortField(
        IQueryable<TruckEntity> query,
        TruckSortField sortField,
        bool thenBy)
    {
        var descending = sortField.Direction == TruckSortDirection.Descending;

        return sortField.Field switch
        {
            "Code" => Apply(
                query,
                x => x.Code,
                descending,
                thenBy),
            "Name" => Apply(
                query,
                x => x.Name,
                descending,
                thenBy),
            "Status" => Apply(
                query,
                x => x.Status.Name,
                descending,
                thenBy),
            "Description" => Apply(
                query,
                x => x.Description,
                descending,
                thenBy),
            _ => Apply(
                query,
                x => x.Code,
                descending,
                thenBy)
        };
    }

    private static IOrderedQueryable<TruckEntity> Apply<TKey>(
        IQueryable<TruckEntity> query,
        System.Linq.Expressions.Expression<Func<TruckEntity, TKey>> keySelector,
        bool descending,
        bool thenBy)
    {
        if (thenBy)
        {
            var orderedQuery = (IOrderedQueryable<TruckEntity>)query;

            return descending
                ? orderedQuery.ThenByDescending(keySelector)
                : orderedQuery.ThenBy(keySelector);
        }

        return descending
            ? query.OrderByDescending(keySelector)
            : query.OrderBy(keySelector);
    }
}
