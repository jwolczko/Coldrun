using Coldrun.Modules.TruckManagement.Application.Ports;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Coldrun.Modules.TruckManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddTruckManagementInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<TruckManagementDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("TruckManagement")));

        services.AddScoped<ITruckRepository, TruckRepository>();
        services.AddScoped<ITruckReadModel, TruckReadModel>();

        return services;
    }
}
