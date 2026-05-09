using Coldrun.BuildingBlocks.Application.Messaging;
using Coldrun.Modules.TruckManagement.Application.Trucks.Commands.ChangeTruckStatus;
using Coldrun.Modules.TruckManagement.Application.Trucks.Commands.CreateTruck;
using Coldrun.Modules.TruckManagement.Application.Trucks.Commands.DeleteTruck;
using Coldrun.Modules.TruckManagement.Application.Trucks.Commands.UpdateTruckDetails;
using Coldrun.Modules.TruckManagement.Application.Trucks.Dtos;
using Coldrun.Modules.TruckManagement.Application.Trucks.Queries.GetTruck;
using Coldrun.Modules.TruckManagement.Application.Trucks.Queries.GetTruckStatus;
using Coldrun.Modules.TruckManagement.Application.Trucks.Queries.GetTruckStatuses;
using Coldrun.Modules.TruckManagement.Application.Trucks.Queries.SearchTrucks;
using Microsoft.Extensions.DependencyInjection;
using QueryPagedResult = Coldrun.Modules.TruckManagement.Application.Trucks.Queries.PagedResult<Coldrun.Modules.TruckManagement.Application.Trucks.Queries.TruckListItemDto>;
using QueryTruckDetailsDto = Coldrun.Modules.TruckManagement.Application.Trucks.Queries.TruckDetailsDto;

namespace Coldrun.Modules.TruckManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddTruckManagementApplication(
        this IServiceCollection services)
    {
        services.AddScoped<
            ICommandHandler<CreateTruckCommand, CreateTruckResult>,
            CreateTruckCommandHandler>();

        services.AddScoped<
            ICommandHandler<UpdateTruckDetailsCommand, UpdateTruckDetailsResult>,
            UpdateTruckDetailsCommandHandler>();

        services.AddScoped<
            ICommandHandler<ChangeTruckStatusCommand, ChangeTruckStatusResult>,
            ChangeTruckStatusCommandHandler>();

        services.AddScoped<
            ICommandHandler<DeleteTruckCommand>,
            DeleteTruckCommandHandler>();

        services.AddScoped<
            IQueryHandler<GetTruckQuery, QueryTruckDetailsDto?>,
            GetTruckQueryHandler>();

        services.AddScoped<
            IQueryHandler<SearchTrucksQuery, QueryPagedResult>,
            SearchTrucksQueryHandler>();

        services.AddScoped<
            IQueryHandler<GetTruckStatusQuery, TruckStatusDto?>,
            GetTruckStatusQueryHandler>();

        services.AddScoped<
            IQueryHandler<GetTruckStatusesQuery, IReadOnlyCollection<TruckStatusMetadataDto>>,
            GetTruckStatusesQueryHandler>();

        return services;
    }
}
