using System;
using System.Collections.Generic;
using System.Text;

namespace Coldrun.Modules.TruckManagement.Application.Trucks.Dtos;

public sealed record TruckDetailsDto(
    string Code,
    string Name,
    string Status,
    string? Description,
    IReadOnlyCollection<string> AllowedStatusTransitions);
