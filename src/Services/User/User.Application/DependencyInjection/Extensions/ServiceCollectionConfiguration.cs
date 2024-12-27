using System.Reflection;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.Middleware;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using User.Application.Mappers;

namespace User.Application.DependencyInjection.Extensions;

public static class ServiceCollectionConfiguration
{
    
    private const string COR_NAME = "user_cors";
    
    
    public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration configuration)
    {
        
        services.AddCorsService();
        services.AddMapperService();
        services.AddMediaRServices();
        return services;
    }
    
    
    private static IServiceCollection AddCorsService(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(name: COR_NAME, builder =>
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
    
    
    private static IServiceCollection AddMediaRServices(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            options.AddOpenBehavior(typeof(ValidationBehavior<,>));
            options.AddOpenBehavior(typeof(PerformancePipelineBehavior<,>));
            options.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        services.AddFeatureManagement();
        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }
    
    public static WebApplication UseApplicationService(this WebApplication app)
    {
        app.UseMiddleware<IpLoggingMiddleware>();
        app.UseExceptionHandler(configs => { });
        app.UseCors(COR_NAME);
        return app;
    }
}