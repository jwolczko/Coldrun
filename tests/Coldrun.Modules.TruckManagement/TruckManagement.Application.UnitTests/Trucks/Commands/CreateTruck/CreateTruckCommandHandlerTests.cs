using Coldrun.Modules.TruckManagement.Application.Trucks.Commands.CreateTruck;
using Coldrun.Modules.TruckManagement.Application.Trucks.Exceptions;
using Coldrun.Modules.TruckManagement.Application.Trucks.Ports;
using Coldrun.Modules.TruckManagement.Domain.Trucks;

namespace Coldrun.Modules.TruckManagement.Application.UnitTests.Trucks.Commands.CreateTruck;

public sealed class CreateTruckCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_WhenTruckCodeAlreadyExists_ThrowsException()
    {
        var repository = new FakeTruckRepository
        {
            ExistsByCodeAsyncResult = true
        };
        var unitOfWork = new FakeTruckManagementUnitOfWork();
        var handler = new CreateTruckCommandHandler(repository, unitOfWork);
        var command = new CreateTruckCommand("TRK-001", "Truck 1", "Loading", "Ready");

        await Assert.ThrowsAsync<TruckCodeAlreadyExistsException>(() =>
            handler.HandleAsync(command, CancellationToken.None));

        Assert.Equal(0, unitOfWork.SaveChangesCalls);
        Assert.Null(repository.AddedTruck);
    }

    [Fact]
    public async Task HandleAsync_WhenCommandIsValid_AddsTruckAndReturnsMappedResult()
    {
        var repository = new FakeTruckRepository();
        var unitOfWork = new FakeTruckManagementUnitOfWork();
        var handler = new CreateTruckCommandHandler(repository, unitOfWork);
        var command = new CreateTruckCommand("TRK-001", "Truck 1", "Loading", "Ready");

        var result = await handler.HandleAsync(command, CancellationToken.None);

        Assert.NotNull(repository.AddedTruck);
        Assert.Equal("TRK-001", repository.AddedTruck!.Code.Value);
        Assert.Equal(1, unitOfWork.SaveChangesCalls);
        Assert.Equal("TRK-001", result.Code);
        Assert.Equal("Truck 1", result.Name);
        Assert.Equal("Loading", result.Status);
        Assert.Equal("Ready", result.Description);
        Assert.Contains("To Job", result.AllowedStatusTransitions);
        Assert.Contains("Out Of Service", result.AllowedStatusTransitions);
    }

    private sealed class FakeTruckRepository : ITruckRepository
    {
        public bool ExistsByCodeAsyncResult { get; set; }
        public Truck? AddedTruck { get; private set; }

        public Task<Truck?> GetByCodeAsync(TruckCode code, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<Truck?>(null);
        }

        public Task<bool> ExistsByCodeAsync(TruckCode code, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(ExistsByCodeAsyncResult);
        }

        public Task AddAsync(Truck truck, CancellationToken cancellationToken = default)
        {
            AddedTruck = truck;
            return Task.CompletedTask;
        }

        public void Remove(Truck truck)
        {
        }
    }

    private sealed class FakeTruckManagementUnitOfWork : ITruckManagementUnitOfWork
    {
        public int SaveChangesCalls { get; private set; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SaveChangesCalls++;
            return Task.FromResult(1);
        }
    }
}
