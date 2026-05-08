using Coldrun.BuildingBlocks.Domain;
using Coldrun.Modules.TruckManagement.Domain.Trucks.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coldrun.Modules.TruckManagement.Domain.Trucks;

public sealed class Truck : AggregateRoot
{
    public TruckCode Code { get; private set; }
    public TruckName Name { get; private set; }
    public TruckStatus Status { get; private set; }
    public TruckDescription? Description { get; private set; }

    public void ChangeStatus(TruckStatus newStatus)
    {
        if (!TruckStatusTransitionPolicy.CanChange(Status, newStatus))
        {
            throw new InvalidTruckStatusTransitionException(Status, newStatus);
        }

        var oldStatus = Status;
        Status = newStatus;

        AddDomainEvent(new TruckStatusChangedDomainEvent(Code, oldStatus, newStatus));
    }
}
