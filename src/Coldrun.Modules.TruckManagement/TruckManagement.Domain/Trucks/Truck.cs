using Coldrun.BuildingBlocks.Domain;

namespace Coldrun.Modules.TruckManagement.Domain.Trucks;

public sealed class Truck : Entity
{
    public TruckCode Code { get; private set; }
    public TruckName Name { get; private set; }
    public TruckStatus Status { get; private set; }
    public TruckDescription? Description { get; private set; }

    public static Truck Create(TruckCode code, TruckName name, TruckStatus status, TruckDescription? description) => new Truck
    {
        Code = code,
        Name = name,
        Status = status,
        Description = description
    };

    public void ChangeStatus(TruckStatus newStatus)
    {
        if (!TruckStatusTransitionPolicy.CanChange(Status, newStatus))
        {
            throw new InvalidTruckStatusTransitionException(Status, newStatus);
        }

        Status = newStatus;
    }

    public void UpdateDetails(TruckName name, TruckDescription? description)
    {
        Name = name;
        Description = description;
    }
}
