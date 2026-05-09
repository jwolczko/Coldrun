namespace Coldrun.Modules.TruckManagement.Domain.Trucks;

public class TruckDescription
{
    public static TruckDescription? CreateOptional(string? description)
    {
        if (description is null) return null;

        return new TruckDescription
        {
            Value = description,
        };
    }
    public string Value { get; set; } = string.Empty;
}
