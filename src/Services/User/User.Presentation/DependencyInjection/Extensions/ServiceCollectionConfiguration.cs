using Microsoft.Extensions.DependencyInjection;

namespace User.Presentation.DependencyInjection.Extensions;

public static class ServiceCollectionConfiguration
{
    public static IServiceCollection AddPresentationService(this IServiceCollection services)
    {
        return services;
    }
}