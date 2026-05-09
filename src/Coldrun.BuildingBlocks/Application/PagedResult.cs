using System;
using System.Collections.Generic;
using System.Text;

namespace Coldrun.BuildingBlocks.Application;

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
