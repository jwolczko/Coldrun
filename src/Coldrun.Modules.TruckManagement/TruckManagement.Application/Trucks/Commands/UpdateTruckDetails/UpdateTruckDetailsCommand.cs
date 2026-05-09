using Coldrun.BuildingBlocks.Application;
using Coldrun.BuildingBlocks.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coldrun.Modules.TruckManagement.Application.Trucks.Commands.UpdateTruckDetails;

public sealed record UpdateTruckDetailsCommand(
    string Code,
    Optional<string> Name,
    Optional<string?> Description
) : ICommand<UpdateTruckDetailsResult>;
