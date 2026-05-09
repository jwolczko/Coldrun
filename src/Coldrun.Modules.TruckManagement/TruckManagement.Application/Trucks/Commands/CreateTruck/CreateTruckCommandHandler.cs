using Coldrun.BuildingBlocks.Application.Messaging;
using Coldrun.Modules.TruckManagement.Application.Trucks.Exceptions;
using Coldrun.Modules.TruckManagement.Application.Trucks.Ports;
using Coldrun.Modules.TruckManagement.Domain.Trucks;

namespace Coldrun.Modules.TruckManagement.Application.Trucks.Commands.CreateTruck;

public sealed class CreateTruckCommandHandler
    : ICommandHandler<CreateTruckCommand, CreateTruckResult>
{
    private readonly ITruckRepository _truckRepository;
    private readonly ITruckManagementUnitOfWork _unitOfWork;

    public CreateTruckCommandHandler(
        ITruckRepository truckRepository,
        ITruckManagementUnitOfWork unitOfWork)
    {
        _truckRepository = truckRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateTruckResult> HandleAsync(
        CreateTruckCommand command,
        CancellationToken cancellationToken = default)
    {
        var code = TruckCode.Create(command.Code);

        var alreadyExists = await _truckRepository.ExistsByCodeAsync(
            code,
            cancellationToken);

        if (alreadyExists)
        {
            throw new TruckCodeAlreadyExistsException(code.Value);
        }

        var name = TruckName.Create(command.Name);
        var status = TruckStatus.From(command.Status);
        var description = TruckDescription.CreateOptional(command.Description);

        var truck = Truck.Create(
            code,
            name,
            status,
            description);

        await _truckRepository.AddAsync(
            truck,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var allowedTransitions = TruckStatusTransitionPolicy
            .GetAllowedTransitions(truck.Status)
            .Select(x => x.Value)
            .ToArray();

        return new CreateTruckResult(
            truck.Code.Value,
            truck.Name.Value,
            truck.Status.Value,
            truck.Description?.Value,
            allowedTransitions);
    }
}