using Coldrun.Modules.TruckManagement.Application.Trucks.Ports;
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

        services.AddScoped<ITruckManagementUnitOfWork>(sp =>
            sp.GetRequiredService<TruckManagementDbContext>());
        services.AddScoped<ITruckRepository, TruckRepository>();
        services.AddScoped<ITruckReadModel, TruckReadModel>();

        return services;
    }
}
