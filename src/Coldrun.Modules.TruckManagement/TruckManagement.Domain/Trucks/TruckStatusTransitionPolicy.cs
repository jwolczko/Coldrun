namespace Coldrun.Modules.TruckManagement.Domain.Trucks;

public static class TruckStatusTransitionPolicy
{
    public static bool CanChange(TruckStatus currentStatus, TruckStatus newStatus)
    {
        ArgumentNullException.ThrowIfNull(currentStatus);
        ArgumentNullException.ThrowIfNull(newStatus);

        return true;
    }
}
