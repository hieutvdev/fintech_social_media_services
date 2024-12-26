using Microsoft.Extensions.DependencyInjection;

namespace User.Infrastructure.DependencyInjection.Extensions;

public static class ServiceCollectionConfiguration
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services)
    {
        return services;
    }
}