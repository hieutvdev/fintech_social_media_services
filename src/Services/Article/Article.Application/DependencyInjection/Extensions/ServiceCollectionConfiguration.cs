using System.Reflection;
using System.Runtime.Intrinsics.X86;
using Article.Application.Mappers;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.Logging;
using BuildingBlocks.Middleware;
using FluentValidation;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;

namespace Article.Application.DependencyInjection.Extensions;

public static class ServiceCollectionConfiguration
{
    private const string CorName = "article_cors";
    
    
    
    public static IServiceCollection AddApplicationService(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddCorsService();
        services.AddMapperService();
        services.AddMediaRService();
        services.AddHealthCheckServices(configuration);
        return services;
    }


    private static IServiceCollection AddCorsService(this IServiceCollection services)
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

        return services;
    }

    private static IServiceCollection AddMapperService(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ServiceProfile));
        return services;
    }


    private static IServiceCollection AddMediaRService(this IServiceCollection services)
    {
        services.AddMediatR(configs =>
        {
            configs.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            configs.AddOpenBehavior(typeof(ValidationBehavior<,>));
            configs.AddOpenBehavior(typeof(PerformancePipelineBehavior<,>));
            configs.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });
        services.AddFeatureManagement();
        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        return services;
    }

    private static IServiceCollection AddHealthCheckServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddRedis(
                $"{configuration.GetSection("RedisConfiguration:Host"!).Value!}:{configuration.GetSection("RedisConfiguration:Port").Value!}")
            .AddNpgSql(configuration.GetConnectionString("Database")!);
           

        return services;

    }

    public static WebApplication UseApplicationService(this WebApplication app)
    {
        app.UseMiddleware<IpLoggingMiddleware>();
        app.UseExceptionHandler(configs => { });
        app.UseCors(CorName);
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        return app;
    }
}