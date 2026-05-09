using Coldrun.BuildingBlocks.Application.Messaging;

namespace Coldrun.Modules.TruckManagement.Api.IntegrationTests.Infrastructure;

public sealed class DelegateCommandHandler<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    private readonly Func<TCommand, CancellationToken, Task<TResponse>> _handler;

    public DelegateCommandHandler(Func<TCommand, CancellationToken, Task<TResponse>> handler)
    {
        _handler = handler;
    }

    public Task<TResponse> HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        return _handler(command, cancellationToken);
    }
}

public sealed class DelegateCommandHandler<TCommand> : ICommandHandler<TCommand>
    where TCommand : ICommand
{
    private readonly Func<TCommand, CancellationToken, Task> _handler;

    public DelegateCommandHandler(Func<TCommand, CancellationToken, Task> handler)
    {
        _handler = handler;
    }

    public Task HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        return _handler(command, cancellationToken);
    }
}

public sealed class DelegateQueryHandler<TQuery, TResponse> : IQueryHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    private readonly Func<TQuery, CancellationToken, Task<TResponse>> _handler;

    public DelegateQueryHandler(Func<TQuery, CancellationToken, Task<TResponse>> handler)
    {
        _handler = handler;
    }

    public Task<TResponse> HandleAsync(TQuery query, CancellationToken cancellationToken = default)
    {
        return _handler(query, cancellationToken);
    }
}
