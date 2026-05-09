namespace Coldrun.Modules.TruckManagement.Domain.Trucks;

public record TruckName
{
    public static TruckName Create(string truckName) => new TruckName { Value = truckName };

    public required string Value { get; set; }
}
