using Coldrun.Modules.TruckManagement.Api.Hypermedia;
using Coldrun.Modules.TruckManagement.Application.Trucks.Commands.CreateTruck;
using Coldrun.Modules.TruckManagement.Application.Trucks.Commands.UpdateTruckDetails;
using Coldrun.Modules.TruckManagement.Application.Trucks.Queries;

namespace Coldrun.Modules.TruckManagement.Api;

public sealed class TruckRepresentationFactory
{
    private readonly TruckLinksFactory _linksFactory;

    public TruckRepresentationFactory(TruckLinksFactory linksFactory)
    {
        _linksFactory = linksFactory;
    }

    public object Create(CreateTruckResult truck)
    {
        return CreateTruckRepresentation(
            truck.Code,
            truck.Name,
            truck.Status,
            truck.Description,
            truck.AllowedStatusTransitions);
    }

    public object Create(UpdateTruckDetailsResult truck)
    {
        return CreateTruckRepresentation(
            truck.Code,
            truck.Name,
            truck.Status,
            truck.Description,
            truck.AllowedStatusTransitions);
    }

    public object Create(TruckDetailsDto truck)
    {
        return CreateTruckRepresentation(
            truck.Code,
            truck.Name,
            truck.Status,
            truck.Description,
            Array.Empty<string>());
    }

    public object CreateCollection(PagedResult<TruckListItemDto> result)
    {
        return new
        {
            items = result.Items.Select(CreateListItem).ToArray(),
            pageNumber = result.PageNumber,
            pageSize = result.PageSize,
            totalElements = result.TotalElements,
            totalPages = result.TotalPages,
            hasPreviousPage = result.HasPreviousPage,
            hasNextPage = result.HasNextPage
        };
    }

    private object CreateListItem(TruckListItemDto truck)
    {
        return new
        {
            code = truck.Code,
            name = truck.Name,
            status = truck.Status,
            description = truck.Description,
            links = _linksFactory.CreateForTruck(truck.Code, Array.Empty<string>())
        };
    }

    private object CreateTruckRepresentation(
        string code,
        string name,
        string status,
        string? description,
        IReadOnlyCollection<string> allowedStatusTransitions)
    {
        return new
        {
            code,
            name,
            status,
            description,
            allowedStatusTransitions,
            links = _linksFactory.CreateForTruck(code, allowedStatusTransitions)
        };
    }
}
