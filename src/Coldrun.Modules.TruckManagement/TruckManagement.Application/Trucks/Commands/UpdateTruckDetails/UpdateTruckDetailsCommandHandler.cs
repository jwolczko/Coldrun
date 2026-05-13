using Coldrun.BuildingBlocks.Application.Messaging;
using Coldrun.Modules.TruckManagement.Application.Trucks.Exceptions;
using Coldrun.Modules.TruckManagement.Application.Trucks.Ports;
using Coldrun.Modules.TruckManagement.Domain.Trucks;

namespace Coldrun.Modules.TruckManagement.Application.Trucks.Commands.UpdateTruckDetails;

public sealed class UpdateTruckDetailsCommandHandler
    : ICommandHandler<UpdateTruckDetailsCommand, UpdateTruckDetailsResult>
{
    private readonly ITruckRepository _truckRepository;
    private readonly ITruckManagementUnitOfWork _unitOfWork;

    public UpdateTruckDetailsCommandHandler(
        ITruckRepository truckRepository,
        ITruckManagementUnitOfWork unitOfWork)
    {
        _truckRepository = truckRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateTruckDetailsResult> HandleAsync(
        UpdateTruckDetailsCommand command,
        CancellationToken cancellationToken = default)
    {
        if (!command.Name.HasValue && !command.Description.HasValue)
        {
            throw new EmptyTruckUpdateException();
        }

        var code = TruckCode.Create(command.Code);

        var truck = await _truckRepository.GetByCodeAsync(
            code,
            cancellationToken);

        if (truck is null)
        {
            throw new TruckNotFoundException(code.Value);
        }

        var name = command.Name.HasValue
            ? TruckName.Create(command.Name.Value!)
            : truck.Name;

        var description = command.Description.HasValue
            ? TruckDescription.CreateOptional(command.Description.Value)
            : truck.Description;

        truck.UpdateDetails(
            name,
            description);

        await _truckRepository.UpdateAsync(
            truck,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var allowedTransitions = TruckStatusTransitionPolicy
            .GetAllowedTransitions(truck.Status)
            .Select(x => x.Value)
            .ToArray();

        return new UpdateTruckDetailsResult(
            truck.Code.Value,
            truck.Name.Value,
            truck.Status.Value,
            truck.Description?.Value,
            allowedTransitions);
    }
}
