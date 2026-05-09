namespace Coldrun.Modules.TruckManagement.Application.Trucks.Exceptions;

public sealed class TruckNotFoundException : Exception
{
    public TruckNotFoundException(string code)
        : base($"Truck with code '{code}' was not found.")
    {
        Code = code;
    }

    public string Code { get; }
}
