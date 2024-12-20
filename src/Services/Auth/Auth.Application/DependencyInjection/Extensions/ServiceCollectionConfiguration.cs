using System.Reflection;
using Auth.Application.Mappers;
using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.Middleware;
using FluentValidation;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;

namespace Auth.Application.DependencyInjection.Extensions;

public static class ServiceCollectionConfiguration
{
    private const string CorName = "authenticaion_cors";
    public static IServiceCollection AddApplicationService(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(name: CorName, builder =>
            {
                builder.WithOrigins("*")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
        services.AddMapperService();


        services.AddMediatR(configs =>
        {
            configs.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            configs.AddOpenBehavior(typeof(ValidationBehavior<,>));
            configs.AddOpenBehavior(typeof(PerformancePipelineBehavior<,>));
        });

        services.AddFeatureManagement();
        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddHealthCheckServices(configuration);
        

        return services;
    }

    
    private static IServiceCollection AddMapperService(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ServiceProfile));
        return services;
    }


    private static IServiceCollection AddHealthCheckServices(this IServiceCollection services,
        IConfiguration configuration)
    {


        services.AddHealthChecks()
            .AddRedis($"{configuration.GetSection("RedisConfiguration:Host"!).Value!}:{configuration.GetSection("RedisConfiguration:Port").Value!}")
            .AddSqlServer(configuration.GetConnectionString("Database")!)
            .AddKafka(options =>
            {
                options.BootstrapServers = configuration.GetSection("Kafka:BootstrapServers").Value;
                
            });
        
        return services;
    }


    public static WebApplication UseApplicationService(this WebApplication app)
    {
        app.UseMiddleware<IpLoggingMiddleware>();
        app.UseExceptionHandler(options => { });
        app.UseCors(CorName);
        app.UseHealthChecks("/health",
            new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        
        return app;
    }
}