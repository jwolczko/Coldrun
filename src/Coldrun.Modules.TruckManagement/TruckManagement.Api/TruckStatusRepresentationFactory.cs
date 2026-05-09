using Coldrun.Modules.TruckManagement.Api.Hypermedia;
using Coldrun.Modules.TruckManagement.Application.Trucks.Commands.ChangeTruckStatus;
using Coldrun.Modules.TruckManagement.Application.Trucks.Dtos;

namespace Coldrun.Modules.TruckManagement.Api;

public sealed class TruckStatusRepresentationFactory
{
    private readonly TruckLinksFactory _linksFactory;

    public TruckStatusRepresentationFactory(TruckLinksFactory linksFactory)
    {
        _linksFactory = linksFactory;
    }

    public object Create(TruckStatusDto truckStatus)
    {
        return CreateRepresentation(
            truckStatus.Code,
            truckStatus.Status,
            truckStatus.AllowedStatusTransitions);
    }

    public object Create(ChangeTruckStatusResult truckStatus)
    {
        return CreateRepresentation(
            truckStatus.Code,
            truckStatus.Status,
            truckStatus.AllowedStatusTransitions);
    }

    private object CreateRepresentation(
        string code,
        string status,
        IReadOnlyCollection<string> allowedStatusTransitions)
    {
        return new
        {
            code,
            status,
            allowedStatusTransitions,
            links = _linksFactory.CreateForTruck(code, allowedStatusTransitions)
        };
    }
}
