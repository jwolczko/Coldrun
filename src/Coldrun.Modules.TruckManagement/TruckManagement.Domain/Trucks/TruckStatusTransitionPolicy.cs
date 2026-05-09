namespace Coldrun.Modules.TruckManagement.Domain.Trucks;

public static class TruckStatusTransitionPolicy
{
    public static bool CanChange(
        TruckStatus currentStatus,
        TruckStatus requestedStatus)
    {
        return CanTransition(currentStatus, requestedStatus);
    }

    public static bool CanTransition(
        TruckStatus currentStatus,
        TruckStatus requestedStatus)
    {
        return GetAllowedTransitions(currentStatus)
            .Contains(requestedStatus);
    }

    public static IReadOnlyCollection<TruckStatus> GetAllowedTransitions(
        TruckStatus currentStatus)
    {
        if (currentStatus == TruckStatus.OutOfService)
        {
            return TruckStatus.GetAll();
        }

        if (currentStatus == TruckStatus.Loading)
        {
            return new[]
            {
                TruckStatus.ToJob,
                TruckStatus.OutOfService
            };
        }

        if (currentStatus == TruckStatus.ToJob)
        {
            return new[]
            {
                TruckStatus.AtJob,
                TruckStatus.OutOfService
            };
        }

        if (currentStatus == TruckStatus.AtJob)
        {
            return new[]
            {
                TruckStatus.Returning,
                TruckStatus.OutOfService
            };
        }

        if (currentStatus == TruckStatus.Returning)
        {
            return new[]
            {
                TruckStatus.Loading,
                TruckStatus.OutOfService
            };
        }

        return Array.Empty<TruckStatus>();
    }
}
