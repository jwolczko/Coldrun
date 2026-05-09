namespace Coldrun.Modules.TruckManagement.Application.Trucks.Exceptions;

public sealed class TruckCodeAlreadyExistsException : Exception
{
    public TruckCodeAlreadyExistsException(string code)
        : base($"Truck with code '{code}' already exists.")
    {
        Code = code;
    }

    public string Code { get; }
}