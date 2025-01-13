using BuildingBlocks.Logging;

namespace WebGateway.DependencyInjection.Extensions;

public static class ServiceCollectionConfiguration
{
    public static IServiceCollection AddServiceCollection(this IServiceCollection service, IConfiguration configuration)
    {
       
        return service;
    }
}