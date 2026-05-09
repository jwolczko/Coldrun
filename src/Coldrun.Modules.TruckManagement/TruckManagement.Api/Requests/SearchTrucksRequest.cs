namespace Coldrun.Modules.TruckManagement.Api.Requests;

public sealed class SearchTrucksRequest
{
    public string? Code { get; set; }
    public string? CodeContains { get; set; }
    public string? NameContains { get; set; }
    public string? Status { get; set; }
    public string? DescriptionContains { get; set; }
    public string? Sort { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
