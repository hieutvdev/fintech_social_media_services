using BuildingBlocks.Logging;
using BuildingBlocks.Messaging.Messaging.Kafka;
using Mail.API.Configurations;
using Mail.API.Services;

namespace Mail.API.DependencyInjection.Extensions;

public static class ServiceCollectionConfiguration
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        var emailSetting = new EmailSettings();
        configuration.GetSection("EmailSettings").Bind(emailSetting);
        services.AddKafkaService(configuration);
        
        return services;
    }
    
    
    private static IServiceCollection AddKafkaService(this IServiceCollection services, IConfiguration configuration)
    {

        services.Configure<KafkaSettings>(configuration.GetSection("Kafka"));
        services.AddSingleton(typeof(IKafkaConsumerService<,>), typeof(KafkaConsumerService<,>));
        services.AddHostedService<KafkaConsumerBackgroundService>();

        return services;
    }


    private static IServiceCollection AddLoggerService(this IServiceCollection services, IConfiguration configuration, ConfigureHostBuilder hostBuilder)
    {
        services.AddSerilogService(configuration, hostBuilder);
        return services;
    }


    public static WebApplication UseApiServices(this WebApplication app)
    {
        return app;
    }
}