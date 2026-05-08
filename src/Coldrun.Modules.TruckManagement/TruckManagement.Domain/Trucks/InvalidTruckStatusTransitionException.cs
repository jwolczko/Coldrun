namespace Coldrun.Modules.TruckManagement.Domain.Trucks;

public sealed class InvalidTruckStatusTransitionException : Exception
{
    public InvalidTruckStatusTransitionException(
        TruckStatus currentStatus,
        TruckStatus newStatus)
        : base($"Truck status transition from '{currentStatus}' to '{newStatus}' is not allowed.")
    {
    }
}
