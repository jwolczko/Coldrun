using Coldrun.BuildingBlocks.Application.Messaging;
using Coldrun.Modules.TruckManagement.Api.Requests;
using Coldrun.Modules.TruckManagement.Application.Trucks.Commands.ChangeTruckStatus;
using Coldrun.Modules.TruckManagement.Application.Trucks.Dtos;
using Coldrun.Modules.TruckManagement.Application.Trucks.Queries.GetTruckStatus;
using Microsoft.AspNetCore.Mvc;

namespace Coldrun.Modules.TruckManagement.Api.Controllers;

[ApiController]
[Route("api/v1/trucks/{code}/status")]
public sealed class TruckStatusesController : ControllerBase
{
    private readonly ICommandHandler<ChangeTruckStatusCommand, ChangeTruckStatusResult> _changeStatusHandler;
    private readonly IQueryHandler<GetTruckStatusQuery, TruckStatusDto?> _getStatusHandler;
    private readonly TruckStatusRepresentationFactory _representationFactory;

    public TruckStatusesController(
        ICommandHandler<ChangeTruckStatusCommand, ChangeTruckStatusResult> changeStatusHandler,
        IQueryHandler<GetTruckStatusQuery, TruckStatusDto?> getStatusHandler,
        TruckStatusRepresentationFactory representationFactory)
    {
        _changeStatusHandler = changeStatusHandler;
        _getStatusHandler = getStatusHandler;
        _representationFactory = representationFactory;
    }

    [HttpGet]
    public async Task<IActionResult> GetStatus(
        string code,
        CancellationToken cancellationToken)
    {
        var result = await _getStatusHandler.HandleAsync(
            new GetTruckStatusQuery(code),
            cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(_representationFactory.Create(result));
    }

    [HttpPut]
    public async Task<IActionResult> ChangeStatus(
        string code,
        [FromBody] ChangeTruckStatusRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ChangeTruckStatusCommand(
            code,
            request.Status);

        var result = await _changeStatusHandler.HandleAsync(
            command,
            cancellationToken);

        return Ok(_representationFactory.Create(result));
    }
}
