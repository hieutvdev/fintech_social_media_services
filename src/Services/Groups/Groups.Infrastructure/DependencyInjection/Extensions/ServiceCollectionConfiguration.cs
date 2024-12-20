using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShredKernel.BaseClasses.Configurations;
using StackExchange.Redis;

namespace Groups.Infrastructure.DependencyInjection.Extensions;

public static class ServiceCollectionConfiguration
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services,
        IConfiguration configuration)
    {

        return services;
    }



    private static IServiceCollection AddDatabaseServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database") ??
                               throw new ArgumentException("ConnectionString is null");

        var redisConfiguration = new RedisConfiguration();
        
        configuration.GetSection("RedisConfiguration").Bind(redisConfiguration);

        services.Configure<RedisConfiguration>(configuration.GetSection("RedisConfiguration"));




        services.AddSingleton(redisConfiguration);

        if (!redisConfiguration.Enabled)
        {
            return services;
        }
        
        services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect($"{redisConfiguration.Host}:{redisConfiguration.Port}"));

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = $"{redisConfiguration.Host}:{redisConfiguration.Port}";
            options.InstanceName = "Article_Cache";
        });
        
        return services;
    }
}