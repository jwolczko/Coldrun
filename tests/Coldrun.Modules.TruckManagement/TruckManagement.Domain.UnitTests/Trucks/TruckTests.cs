using Coldrun.Modules.TruckManagement.Domain.Trucks;

namespace Coldrun.Modules.TruckManagement.Domain.UnitTests.Trucks;

public sealed class TruckTests
{
    [Fact]
    public void ChangeStatus_WhenTransitionIsAllowed_UpdatesStatus()
    {
        var truck = Truck.Create(
            TruckCode.Create("TRK-001"),
            TruckName.Create("Truck 1"),
            TruckStatus.Loading,
            TruckDescription.CreateOptional("Ready"));

        truck.ChangeStatus(TruckStatus.ToJob);

        Assert.Equal(TruckStatus.ToJob, truck.Status);
    }

    [Fact]
    public void ChangeStatus_WhenTransitionIsNotAllowed_ThrowsException()
    {
        var truck = Truck.Create(
            TruckCode.Create("TRK-001"),
            TruckName.Create("Truck 1"),
            TruckStatus.Loading,
            TruckDescription.CreateOptional("Ready"));

        var action = () => truck.ChangeStatus(TruckStatus.AtJob);

        Assert.Throws<InvalidTruckStatusTransitionException>(action);
    }

    [Fact]
    public void UpdateDetails_WhenCalled_UpdatesNameAndDescription()
    {
        var truck = Truck.Create(
            TruckCode.Create("TRK-001"),
            TruckName.Create("Truck 1"),
            TruckStatus.Loading,
            TruckDescription.CreateOptional("Before"));

        truck.UpdateDetails(
            TruckName.Create("Truck 2"),
            TruckDescription.CreateOptional("After"));

        Assert.Equal("Truck 2", truck.Name.Value);
        Assert.Equal("After", truck.Description?.Value);
    }
}
