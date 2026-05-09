namespace Coldrun.Modules.TruckManagement.Domain.Trucks;

public sealed record TruckStatus
{
    public static readonly TruckStatus OutOfService = new("Out Of Service");
    public static readonly TruckStatus Loading = new("Loading");
    public static readonly TruckStatus ToJob = new("To Job");
    public static readonly TruckStatus AtJob = new("At Job");
    public static readonly TruckStatus Returning = new("Returning");

    private TruckStatus(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static TruckStatus From(string value)
    {
        var normalized = value.Trim();

        return GetAll().FirstOrDefault(x =>
            string.Equals(x.Value, normalized, StringComparison.OrdinalIgnoreCase))
            ?? throw new InvalidTruckStatusException(value);
    }

    public static IReadOnlyCollection<TruckStatus> GetAll()
    {
        return new[]
        {
            OutOfService,
            Loading,
            ToJob,
            AtJob,
            Returning
        };
    }

    public override string ToString()
    {
        return Value;
    }
}