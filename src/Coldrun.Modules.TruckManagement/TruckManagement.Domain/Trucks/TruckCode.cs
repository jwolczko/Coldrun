namespace Coldrun.Modules.TruckManagement.Domain.Trucks;

public class TruckCode
{
    public static TruckCode Create(string truckCode) => new TruckCode { Value = truckCode };
    
    public required string Value { get; set; }
}
