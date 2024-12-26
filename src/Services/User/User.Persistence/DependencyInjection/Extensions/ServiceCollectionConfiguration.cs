using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using User.Application.Data;
using User.Persistence.Data;
using User.Persistence.Data.Interceptors;

namespace User.Persistence.DependencyInjection.Extensions;

public static class ServiceCollectionConfiguration
{
    public static IServiceCollection AddPersistenceService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextService(configuration);
        
        return services;
    }
    
    
    private static IServiceCollection AddDbContextService(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("Connection string is null or empty");
        }

        services.AddScoped<ISaveChangesInterceptor, AuditableInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            
            options.UseNpgsql(connectionString, npgsqlOptionsAction: op =>
            {
                op.EnableRetryOnFailure(
                    maxRetryCount: 5,           
                    maxRetryDelay: TimeSpan.FromSeconds(10), 
                    errorCodesToAdd: null);    
            })
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging();

        });

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            
        
        return services;
    }
}