namespace Coldrun.Modules.TruckManagement.Infrastructure;

public sealed class TruckEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Description { get; set; }
}
