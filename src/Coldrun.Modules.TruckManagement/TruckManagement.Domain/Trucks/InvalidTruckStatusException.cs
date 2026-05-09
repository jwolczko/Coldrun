namespace Coldrun.Modules.TruckManagement.Domain.Trucks;

public sealed class InvalidTruckStatusException : Exception
{
    public InvalidTruckStatusException(string status)
        : base($"Truck status '{status}' is invalid.")
    {
    }
}
