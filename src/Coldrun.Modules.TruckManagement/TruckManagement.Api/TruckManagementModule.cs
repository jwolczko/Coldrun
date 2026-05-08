using Coldrun.Modules.TruckManagement.Application;
using Coldrun.Modules.TruckManagement.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Coldrun.Modules.TruckManagement.Api;

public static class TruckManagementModule
{
    public static IServiceCollection AddTruckManagementModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTruckManagementApplication();
        services.AddTruckManagementInfrastructure(configuration);

        return services;
    }

    public static IEndpointRouteBuilder MapTruckManagementModule(
        this IEndpointRouteBuilder app)
    {
        app.MapTrucksEndpoints();

        return app;
    }
}
