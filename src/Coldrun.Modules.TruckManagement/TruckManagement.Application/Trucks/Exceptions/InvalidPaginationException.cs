namespace Coldrun.Modules.TruckManagement.Application.Trucks.Exceptions;

public sealed class InvalidPaginationException : Exception
{
    public InvalidPaginationException(string message)
        : base(message)
    {
    }
}
