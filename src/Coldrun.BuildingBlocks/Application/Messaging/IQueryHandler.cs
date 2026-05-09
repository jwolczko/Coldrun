using System;
using System.Collections.Generic;
using System.Text;

namespace Coldrun.BuildingBlocks.Application.Messaging;

public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    Task<TResponse> HandleAsync(
        TQuery query,
        CancellationToken cancellationToken = default);
}