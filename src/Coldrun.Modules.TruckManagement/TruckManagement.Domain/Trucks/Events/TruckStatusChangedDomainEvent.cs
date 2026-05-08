using Coldrun.BuildingBlocks.Domain;
using Coldrun.Modules.TruckManagement.Domain.Trucks;

namespace Coldrun.Modules.TruckManagement.Domain.Trucks.Events;

public sealed class TruckStatusChangedDomainEvent : DomainEvent
{
    public TruckStatusChangedDomainEvent(
        TruckCode truckCode,
        TruckStatus oldStatus,
        TruckStatus newStatus)
    {
        TruckCode = truckCode;
        OldStatus = oldStatus;
        NewStatus = newStatus;
    }

    public TruckCode TruckCode { get; }
    public TruckStatus OldStatus { get; }
    public TruckStatus NewStatus { get; }
}
