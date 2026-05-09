using Coldrun.BuildingBlocks.Application.Messaging;
using Coldrun.Modules.TruckManagement.Application.Trucks.Exceptions;
using Coldrun.Modules.TruckManagement.Application.Trucks.Ports;
using Coldrun.Modules.TruckManagement.Domain.Trucks;

namespace Coldrun.Modules.TruckManagement.Application.Trucks.Commands.DeleteTruck;

public sealed class DeleteTruckCommandHandler
    : ICommandHandler<DeleteTruckCommand>
{
    private readonly ITruckRepository _truckRepository;
    private readonly ITruckManagementUnitOfWork _unitOfWork;

    public DeleteTruckCommandHandler(
        ITruckRepository truckRepository,
        ITruckManagementUnitOfWork unitOfWork)
    {
        _truckRepository = truckRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(
        DeleteTruckCommand command,
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

        /*
         * Jeżeli w domenie dodasz metodę:
         *
         * truck.Delete();
         *
         * która podnosi TruckDeletedDomainEvent,
         * to wywołaj ją przed Remove().
         */

        _truckRepository.Remove(truck);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
