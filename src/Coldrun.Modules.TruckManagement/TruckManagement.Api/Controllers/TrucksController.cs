using Coldrun.BuildingBlocks.Application.Messaging;
using Coldrun.Modules.TruckManagement.Api.Requests;
using Coldrun.Modules.TruckManagement.Application.Trucks.Commands.CreateTruck;
using Coldrun.Modules.TruckManagement.Application.Trucks.Commands.DeleteTruck;
using Coldrun.Modules.TruckManagement.Application.Trucks.Commands.UpdateTruckDetails;
using Coldrun.Modules.TruckManagement.Application.Trucks.Queries;
using Coldrun.Modules.TruckManagement.Application.Trucks.Queries.GetTruck;
using Coldrun.Modules.TruckManagement.Application.Trucks.Queries.SearchTrucks;
using Microsoft.AspNetCore.Mvc;

namespace Coldrun.Modules.TruckManagement.Api.Controllers;

[ApiController]
[Route("api/v1/trucks")]
public class TrucksController : ControllerBase
{
    private readonly ICommandHandler<CreateTruckCommand, CreateTruckResult> _createTruckHandler;
    private readonly ICommandHandler<UpdateTruckDetailsCommand, UpdateTruckDetailsResult> _updateTruckDetailsHandler;
    private readonly ICommandHandler<DeleteTruckCommand> _deleteTruckHandler;
    private readonly IQueryHandler<GetTruckQuery, TruckDetailsDto?> _getTruckHandler;
    private readonly IQueryHandler<SearchTrucksQuery, PagedResult<TruckListItemDto>> _searchTrucksHandler;
    private readonly TruckRepresentationFactory _representationFactory;

    public TrucksController(
        ICommandHandler<CreateTruckCommand, CreateTruckResult> createTruckHandler,
        ICommandHandler<UpdateTruckDetailsCommand, UpdateTruckDetailsResult> updateTruckDetailsHandler,
        ICommandHandler<DeleteTruckCommand> deleteTruckHandler,
        IQueryHandler<GetTruckQuery, TruckDetailsDto?> getTruckHandler,
        IQueryHandler<SearchTrucksQuery, PagedResult<TruckListItemDto>> searchTrucksHandler,
        TruckRepresentationFactory representationFactory)
    {
        _createTruckHandler = createTruckHandler;
        _updateTruckDetailsHandler = updateTruckDetailsHandler;
        _deleteTruckHandler = deleteTruckHandler;
        _getTruckHandler = getTruckHandler;
        _searchTrucksHandler = searchTrucksHandler;
        _representationFactory = representationFactory;
    }

    [HttpGet]
    public async Task<IActionResult> Search(
        [FromQuery] SearchTrucksRequest request,
        CancellationToken cancellationToken)
    {
        var query = new SearchTrucksQuery(
            request.Code,
            request.CodeContains,
            request.NameContains,
            request.Status,
            request.DescriptionContains,
            request.Sort,
            request.PageNumber,
            request.PageSize);

        var result = await _searchTrucksHandler.HandleAsync(
            query,
            cancellationToken);

        return Ok(_representationFactory.CreateCollection(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateTruckRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateTruckCommand(
            request.Code,
            request.Name,
            request.Status,
            request.Description);

        var result = await _createTruckHandler.HandleAsync(
            command,
            cancellationToken);

        var representation = _representationFactory.Create(result);

        return Created(
            $"/api/v1/trucks/{result.Code}",
            representation);
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> GetByCode(
        string code,
        CancellationToken cancellationToken)
    {
        var query = new GetTruckQuery(code);

        var result = await _getTruckHandler.HandleAsync(
            query,
            cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(_representationFactory.Create(result));
    }

    [HttpPatch("{code}")]
    public async Task<IActionResult> UpdateDetails(
        string code,
        [FromBody] UpdateTruckDetailsRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateTruckDetailsCommand(
            code,
            request.Name,
            request.Description);

        var result = await _updateTruckDetailsHandler.HandleAsync(
            command,
            cancellationToken);

        return Ok(_representationFactory.Create(result));
    }

    [HttpDelete("{code}")]
    public async Task<IActionResult> Delete(
        string code,
        CancellationToken cancellationToken)
    {
        var command = new DeleteTruckCommand(code);

        await _deleteTruckHandler.HandleAsync(
            command,
            cancellationToken);

        return NoContent();
    }
}

