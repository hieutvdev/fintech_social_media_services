using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace User.Application.DependencyInjection.Extensions;

public static class ServiceCollectionConfiguration
{
    
    private const string COR_NAME = "user_cors";
    
    
    public static IServiceCollection AddApplicationService(this IServiceCollection services)
    {
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
    
    
    public static IServiceCollection AddMediaRServices(this IServiceCollection services)
    {
        return services;
    }
    
    public static WebApplication UseApplicationService(this WebApplication app)
    {
        return app;
    }
}