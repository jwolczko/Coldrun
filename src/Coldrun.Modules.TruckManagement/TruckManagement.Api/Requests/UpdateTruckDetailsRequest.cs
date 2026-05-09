using Coldrun.BuildingBlocks.Application;

namespace Coldrun.Modules.TruckManagement.Api.Requests;

public sealed class UpdateTruckDetailsRequest
{
    public Optional<string> Name { get; set; }
    public Optional<string?> Description { get; set; }
}
