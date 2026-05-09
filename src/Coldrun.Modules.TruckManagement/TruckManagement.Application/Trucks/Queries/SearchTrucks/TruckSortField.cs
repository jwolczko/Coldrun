namespace Coldrun.Modules.TruckManagement.Application.Trucks.Queries.SearchTrucks;

public sealed record TruckSortField(
    string Field,
    TruckSortDirection Direction);