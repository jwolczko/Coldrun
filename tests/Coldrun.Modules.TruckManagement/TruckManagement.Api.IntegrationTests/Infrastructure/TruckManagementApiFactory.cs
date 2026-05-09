using Coldrun.BuildingBlocks.Application.Messaging;
using Coldrun.Modules.TruckManagement.Application.Trucks.Commands.ChangeTruckStatus;
using Coldrun.Modules.TruckManagement.Application.Trucks.Commands.CreateTruck;
using Coldrun.Modules.TruckManagement.Application.Trucks.Commands.DeleteTruck;
using Coldrun.Modules.TruckManagement.Application.Trucks.Commands.UpdateTruckDetails;
using Coldrun.Modules.TruckManagement.Application.Trucks.Dtos;
using Coldrun.Modules.TruckManagement.Application.Trucks.Queries;
using Coldrun.Modules.TruckManagement.Application.Trucks.Queries.GetTruck;
using Coldrun.Modules.TruckManagement.Application.Trucks.Queries.GetTruckStatus;
using Coldrun.Modules.TruckManagement.Application.Trucks.Queries.SearchTrucks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using QueryPagedResult = Coldrun.Modules.TruckManagement.Application.Trucks.Queries.PagedResult<Coldrun.Modules.TruckManagement.Application.Trucks.Queries.TruckListItemDto>;
using QueryTruckDetailsDto = Coldrun.Modules.TruckManagement.Application.Trucks.Queries.TruckDetailsDto;

namespace Coldrun.Modules.TruckManagement.Api.IntegrationTests.Infrastructure;

public sealed class TruckManagementApiFactory : WebApplicationFactory<Program>
{
    private readonly Action<IServiceCollection>? _configureServices;

    public TruckManagementApiFactory(Action<IServiceCollection>? configureServices = null)
    {
        _configureServices = configureServices;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<ICommandHandler<CreateTruckCommand, CreateTruckResult>>();
            services.RemoveAll<ICommandHandler<UpdateTruckDetailsCommand, UpdateTruckDetailsResult>>();
            services.RemoveAll<ICommandHandler<DeleteTruckCommand>>();
            services.RemoveAll<ICommandHandler<ChangeTruckStatusCommand, ChangeTruckStatusResult>>();
            services.RemoveAll<IQueryHandler<GetTruckQuery, QueryTruckDetailsDto?>>();
            services.RemoveAll<IQueryHandler<SearchTrucksQuery, QueryPagedResult>>();
            services.RemoveAll<IQueryHandler<GetTruckStatusQuery, TruckStatusDto?>>();

            _configureServices?.Invoke(services);
        });
    }
}
