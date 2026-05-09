using Coldrun.BuildingBlocks.Application.Messaging;
using Coldrun.Modules.TruckManagement.Application.Trucks.Exceptions;
using Coldrun.Modules.TruckManagement.Application.Trucks.Ports;
using Coldrun.Modules.TruckManagement.Domain.Trucks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coldrun.Modules.TruckManagement.Application.Trucks.Commands.ChangeTruckStatus;

public sealed class ChangeTruckStatusCommandHandler
    : ICommandHandler<ChangeTruckStatusCommand, ChangeTruckStatusResult>
{
    private readonly ITruckRepository _truckRepository;
    private readonly ITruckManagementUnitOfWork _unitOfWork;

    public ChangeTruckStatusCommandHandler(
        ITruckRepository truckRepository,
        ITruckManagementUnitOfWork unitOfWork)
    {
        _truckRepository = truckRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ChangeTruckStatusResult> HandleAsync(
        ChangeTruckStatusCommand command,
        CancellationToken cancellationToken = default)
    {
        var code = TruckCode.Create(command.Code);

        var truck = await _truckRepository.GetByCodeAsync(
            code,
            cancellationToken);

        if (truck is null)
        {
            throw new TruckNotFoundException(code.Value);
        }

        var requestedStatus = TruckStatus.From(command.Status);

        truck.ChangeStatus(requestedStatus);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var allowedTransitions = TruckStatusTransitionPolicy
            .GetAllowedTransitions(truck.Status)
            .Select(x => x.Value)
            .ToArray();

        return new ChangeTruckStatusResult(
            truck.Code.Value,
            truck.Status.Value,
            allowedTransitions);
    }
}
