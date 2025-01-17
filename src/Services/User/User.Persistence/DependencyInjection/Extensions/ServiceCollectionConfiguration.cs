using BuildingBlocks.Repository.EntityFrameworkBase.MultipleContext;
using BuildingBlocks.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using User.Application.Data;
using User.Application.Repositories;
using User.Persistence.Data;
using User.Persistence.Data.Interceptors;
using User.Persistence.Repositories;

namespace User.Persistence.DependencyInjection.Extensions;

public static class ServiceCollectionConfiguration
{
    public static IServiceCollection AddPersistenceService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();
        services.AddDbContextService(configuration);
        services.AddRepositoryService();
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
    
    
    private static IServiceCollection AddRepositoryService(this IServiceCollection services)
    {
        services.AddScoped<IAuthorizeExtension, AuthorizeExtension>();
        services.AddScoped(typeof(IRepositoryBaseService<>), typeof(RepositoryBaseService<>));
        services.AddScoped<IUserTypeRepository, UserTypeRepository>();
        services.AddScoped<IUserInfoRepository, UserInfoRepository>();
        services.AddScoped<IFriendRequestRepository, FriendRequestRepository>();
        services.AddScoped<IFriendShipRepository, FriendShipRepository>();
        services.AddScoped<IBlockRepository, BlockRepository>();
        return services;
    }
}