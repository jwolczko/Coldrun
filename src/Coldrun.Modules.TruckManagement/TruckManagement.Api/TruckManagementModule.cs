using Coldrun.Modules.TruckManagement.Application;
using Coldrun.Modules.TruckManagement.Infrastructure;
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

        services.AddScoped<Hypermedia.TruckLinksFactory>();
        services.AddScoped<TruckRepresentationFactory>();
        services.AddScoped<TruckStatusRepresentationFactory>();

        return services;
    }
}
