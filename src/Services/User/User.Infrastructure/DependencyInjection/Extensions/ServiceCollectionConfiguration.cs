using BuildingBlocks.DependencyInjection.Extensions;
using BuildingBlocks.Messaging.Messaging.Kafka;
using BuildingBlocks.Repository.ClientCallApi;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using User.Application.Services;
using User.Infrastructure.Configuration;
using User.Infrastructure.Services;

namespace User.Infrastructure.DependencyInjection.Extensions;

public static class ServiceCollectionConfiguration
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
    {
        
        services.AddAuthenticationService(configuration);
        services.AddDistributedCacheService(configuration);
        services.AddHealthCheckServices(configuration);
        services.AddServiceUseCase(configuration);
        return services;
    }
    
    
    private static IServiceCollection AddHealthCheckServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddRedis(
                $"{configuration.GetSection("RedisConfiguration:Host"!).Value!}:{configuration.GetSection("RedisConfiguration:Port").Value!}")
            .AddNpgSql(configuration.GetConnectionString("Database")!);
            // .AddKafka(options =>
            // {
            //     options.BootstrapServers = configuration.GetSection("Kafka:BootstrapServers").Value;
            //     
            // });

        return services;
    }
    
    
    public static WebApplication UseInfrastructureService(this WebApplication app)
    {
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        return app;
    }
    
    private static IServiceCollection AddMessageBrokerService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(typeof(IKafkaConsumerService<,>), typeof(KafkaConsumerService<,>));
        services.AddSingleton(typeof(IKafkaProducerService<,>), typeof(KafkaProducerService<,>));
        return services;
    }
    
    private static IServiceCollection AddServiceUseCase(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MessageTopic>(configuration.GetSection("MessageTopic"));
        services.AddHttpClient<IClientCallApi, ClientCallApi>();
        services.AddScoped<IAuthServerService, AuthServerService>();
        services.AddScoped<IUserTypeService, UserTypeService>();
        services.AddScoped<IUserInfoService, UserInfoService>();
        services.AddScoped<IFriendRequestService, FriendRequestService>();
        return services;
    }
    

}