namespace Coldrun.Modules.TruckManagement.Infrastructure;

public sealed class TruckEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public short StatusId { get; set; }
    public string? Description { get; set; }

    public TruckStatusEntity Status { get; set; } = null!;
}
