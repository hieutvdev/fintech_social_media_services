using System.Reflection;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.Middleware;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;

namespace Groups.Application.DependencyInjection.Extensions;

public static class ServiceCollectionConfiguration
{
    private const string GROUP_CORS = "GROUP_CORS";


    public static IServiceCollection AddApplicationService(this IServiceCollection services,
        IConfiguration configuration)
    {


        return services;
    }


    private static IServiceCollection AddCorsService(this IServiceCollection services)
    {
        services.AddCors(optionss =>
        {
            optionss.AddPolicy(name: GROUP_CORS, builder =>
            {
                builder.WithOrigins("*")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        return services;
    }

    // private static IServiceCollection AddMapperService(this IServiceCollection services)
    // {
    //     services.AddAutoMapper(typeof(ServiceProfile));
    //     return services;
    // }


    private static IServiceCollection AddMediaRService(this IServiceCollection services, IConfiguration configuration)
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


    private static IServiceCollection AddHealthyCheckService(this IServiceCollection services,
        IConfiguration configuration)
    {

        services.AddHealthChecks();
        return services;
    }



    public static WebApplication UseApplicationService(this WebApplication app)
    {
        app.UseMiddleware<IpLoggingMiddleware>();
        app.UseExceptionHandler(configs => { });
        app.UseCors(GROUP_CORS);
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            //ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return app;
    }
    
}