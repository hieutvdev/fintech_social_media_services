using System.Text;
using BuildingBlocks.DependencyInjection.Options;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Repository.Cache;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using ShredKernel.BaseClasses.Configurations;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

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

    public static IServiceCollection AddSwaggerOptions(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter thr bearer authorization token: `Bearer Generated-JWT-Token`",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    new string[] { }
                }
            });
        });

        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigurationSwaggerOptions>();

        return services;
    }


    public static IServiceCollection AddAuthenticationService(this IServiceCollection services,
        IConfiguration configuration)
    {

        services.AddHttpContextAccessor();
        var jwtToken = new JwtConfiguration();
        configuration.GetSection("JwtConfiguration").Bind(jwtToken);

        var key = Encoding.UTF8.GetBytes(jwtToken.Secret);
        var signingKey = new SymmetricSecurityKey(key);
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = jwtToken.Issuer,
                ValidAudience = jwtToken.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            x.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception is SecurityTokenExpiredException)
                    {
                        Log.Error("Token invalid");
                        throw new UnauthorizedException("Token-Expired");
                    }
                    else
                    {
                        Log.Error("Token invalid");
                        throw new UnauthorizedException("Token-Invalid");
                    }
                }
            };
        });
        return services;
    }

    public static IServiceCollection AddApiVersioningService(this IServiceCollection services)
    {
        services.AddApiVersioning(options => options.ReportApiVersions = true)
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        return services;
    }


public static WebApplication UseCompressionService(this WebApplication app)
    {
        app.UseResponseCompression();
        return app;
    }


    public static WebApplication UseSwaggerOption(this WebApplication app)
    {
        app.UseSwagger();

        app.UseSwaggerUI(options =>
        {
            foreach (var version in app.DescribeApiVersions().Select(version => version.GroupName))
                options.SwaggerEndpoint($"/swagger/{version}/swagger.json", version);
            
            options.DisplayRequestDuration();
            options.EnableTryItOutByDefault();
            options.DocExpansion(DocExpansion.None);
        });

        app.MapGet("/", () => Results.Redirect("swagger/index.html")).WithTags(string.Empty);

        return app;
    }

}