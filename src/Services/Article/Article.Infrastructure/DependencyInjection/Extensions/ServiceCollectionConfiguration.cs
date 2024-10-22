using Article.Application.Data;
using Article.Application.Repositories;
using Article.Application.Services;
using Article.Infrastructure.Data;
using Article.Infrastructure.Data.Interceptors;
using Article.Infrastructure.Repositories;
using Article.Infrastructure.Services;
using BuildingBlocks.DependencyInjection.Extensions;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Repository.EntityFrameworkBase.SingleContext;
using BuildingBlocks.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ShredKernel.BaseClasses.Configurations;
using StackExchange.Redis;

namespace Article.Infrastructure.DependencyInjection.Extensions;

public static class ServiceCollectionConfiguration
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDatabaseServices(configuration);
        services.RegisterRepos();
        services.RegisterServices();
        services.AddAuthenticationService(configuration);
        
        return services;
    }


    private static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database") ?? throw new BadRequestException("ConnectionString is null");
        var redisConfiguration = new RedisConfiguration();
        configuration.GetSection("RedisConfiguration").Bind(redisConfiguration);
        services.Configure<RedisConfiguration>(configuration.GetSection("RedisConfiguration"));

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,           
                        maxRetryDelay: TimeSpan.FromSeconds(10), 
                        errorCodesToAdd: null);       
                })
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging();
        });
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddSingleton(redisConfiguration);
        if (!redisConfiguration.Enabled)
        {
            return services;
        }

        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect($"{redisConfiguration.Host}:{redisConfiguration.Port}"));

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = $"{redisConfiguration.Host}:{redisConfiguration.Port}";
            options.InstanceName = "Article_Cache";
        });
        return services;
    }


    private static IServiceCollection RegisterRepos(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepositoryService<,,>), typeof(RepositoryService<,,>));
        //services.AddTransient<IRepositoryServiceV2, RepositoryServiceV2>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        return services;
    }
    
    
    private static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IAuthorizeExtension, AuthorizeExtension>();
        return services;
    }
}