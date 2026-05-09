using System;
using System.Collections.Generic;
using System.Text;

namespace Coldrun.Modules.TruckManagement.Application.Trucks.Dtos;

public sealed record TruckStatusDto(
    string Code,
    string Status,
    IReadOnlyCollection<string> AllowedStatusTransitions);
