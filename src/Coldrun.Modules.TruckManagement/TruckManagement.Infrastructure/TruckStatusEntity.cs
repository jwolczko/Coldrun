namespace Coldrun.Modules.TruckManagement.Infrastructure;

public sealed class TruckStatusEntity
{
    public short Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public short SortOrder { get; set; }

    public ICollection<TruckEntity> Trucks { get; set; } = [];
}
