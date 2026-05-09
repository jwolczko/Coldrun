namespace Coldrun.Modules.TruckManagement.Application.Trucks.Exceptions;

public sealed class InvalidTruckSortFieldException : Exception
{
    public InvalidTruckSortFieldException(string field)
        : base($"Truck list cannot be sorted by field '{field}'.")
    {
        Field = field;
    }

    public string Field { get; }
}