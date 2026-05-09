namespace Coldrun.Modules.TruckManagement.Api.Requests;

public sealed class CreateTruckRequest
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Description { get; set; }
}
