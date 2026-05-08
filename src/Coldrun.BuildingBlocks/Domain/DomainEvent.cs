namespace Coldrun.BuildingBlocks.Domain;

public abstract class DomainEvent
{
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
}
