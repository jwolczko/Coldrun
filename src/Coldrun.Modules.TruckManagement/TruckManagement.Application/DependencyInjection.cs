using Microsoft.Extensions.DependencyInjection;

namespace Coldrun.Modules.TruckManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddTruckManagementApplication(
        this IServiceCollection services)
    {
        // Tu dodasz handlery, walidatory, serwisy aplikacyjne.
        return services;
    }
}
