namespace Coldrun.Modules.TruckManagement.Application.Trucks.Exceptions;

public sealed class EmptyTruckUpdateException : Exception
{
    public EmptyTruckUpdateException()
        : base("At least one truck field must be provided for update.")
    {
    }
}