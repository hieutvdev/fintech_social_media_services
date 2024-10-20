using BuildingBlocks.Repository.Cache;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace BuildingBlocks.DependencyInjection.Extensions;

public static class ServiceCollectionConfiguration
{
    public static IServiceCollection AddCacheService(this IServiceCollection services, IConfiguration configuration)
    {
        var cacheType = new CacheSetting();
        configuration.GetSection("CacheSettings").Bind(cacheType);
            
        switch (cacheType.CacheType)
        {
            case nameof(CacheOptions.Redis):
                var redisSetting = new RedisCacheSetting();
                configuration.GetSection("CacheSetting:Redis").Bind(redisSetting);
                services.AddSingleton(redisSetting);
                if (!redisSetting.Enabled)
                    return services;
                services.AddSingleton<IConnectionMultiplexer>(_ =>
                    ConnectionMultiplexer.Connect(redisSetting.Connection));

                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisSetting.Connection;
                    options.InstanceName = redisSetting.InstanceName;
                });
                services.AddScoped<RedisCacheService>();
                return services;
            case nameof(CacheOptions.Memory):
                services.AddMemoryCache();
                services.AddScoped<MemoryCacheService>();
                return services;
            
            default:
                return services;
        }
    }

    public static IServiceCollection AddCompressionService(this IServiceCollection services)
    {
        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
            {
                "application/json",
                "multipart/form-data",
                "application/pdf",
                "image/jpeg",
                "image/png",
                "application/zip",
                "application/octet-stream"
            });
        });

        return services;
    }



    public static WebApplication UseCompressionService(this WebApplication app)
    {
        app.UseResponseCompression();
        return app;
    }
    
    
}