using System;
using System.Collections.Generic;
using System.Text;

namespace Coldrun.Modules.TruckManagement.Application.Trucks.Commands.UpdateTruckDetails;

public sealed record UpdateTruckDetailsResult(
    string Code,
    string Name,
    string Status,
    string? Description,
    IReadOnlyCollection<string> AllowedStatusTransitions);
