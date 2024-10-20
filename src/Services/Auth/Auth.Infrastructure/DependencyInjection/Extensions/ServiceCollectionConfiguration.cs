using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Application.Data;
using Auth.Application.Repositories;
using Auth.Application.Services;
using Auth.Domain.Models;
using Auth.Infrastructure.Configuration;
using Auth.Infrastructure.Data;
using Auth.Infrastructure.Data.Interceptors;
using Auth.Infrastructure.Repositories;
using Auth.Infrastructure.Services;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Messaging.Messaging.Kafka;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ShredKernel.BaseClasses.Configurations;
using StackExchange.Redis;


namespace Auth.Infrastructure.DependencyInjection.Extensions;

public static class ServiceCollectionConfiguration
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IRoleService, RoleService>();
        


        services.AddRedisDbService(configuration);
        services.AddDatabaseService(configuration);
        services.AddKafkaService(configuration);
        services.AddAuthenticationServices(configuration);
        
        
        
        return services;
    }

    private static IServiceCollection AddDatabaseService(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new BadRequestException("ConnectionString Db is null or whitespace");
        }
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(connectionString);
        });
        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        

        return services;
    }

    private static IServiceCollection AddRedisDbService(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConfiguration = new RedisConfiguration();
        configuration.GetSection("RedisConfiguration").Bind(redisConfiguration);
        if (!redisConfiguration.Enabled)
        {
            throw new BadRequestException("Redis is closed");
        }

        services.AddSingleton(redisConfiguration);
        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect($"{redisConfiguration.Host}:{redisConfiguration.Port}"));
        services.AddStackExchangeRedisCache(options =>
            options.Configuration = $"{redisConfiguration.Host}:{redisConfiguration.Port}");

        return services;
    }
    private static string GetDeviceInfo(HttpContext context)
    {
        var ipAddress = context!.Connection.RemoteIpAddress?.ToString();
        var userAgent = context.Request.Headers.UserAgent.ToString();
        string deviceInfor = $"{ipAddress}-{userAgent}";

        while (deviceInfor.Contains(" ")) {
            deviceInfor = deviceInfor.Replace(" ", "");
        }
        return deviceInfor;
    }

    private static IServiceCollection AddJwtTokenService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        var jwtToken = new JwtConfiguration();


        configuration.GetSection("JwtConfiguration").Bind(jwtToken);
        var key = Encoding.UTF8.GetBytes(jwtToken.Secret);

        var singingKey = new SymmetricSecurityKey(key);
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = singingKey,
                ValidateIssuer = true,
                ValidIssuer= jwtToken.Issuer,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidAudience= jwtToken.Audience,
                ClockSkew = TimeSpan.Zero,

            };

            x.Events = new JwtBearerEvents
            {
                // OnTokenValidated = async context =>
                // {
                //     var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
                //     var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                //     if (userIdClaim == null)
                //     {
                //         throw new SecurityTokenException("Invalid token. User ID claim is missing.");
                //     }
                //     var userId = userIdClaim.Value;
                //     var cacheKey = $"{CacheKey.Domain}{CacheKey.Auth.AccessToken}{userId}{GetDeviceInfo(context.HttpContext)}";
                //     var tokenExists = await cacheService.GetCacheAsync(cacheKey);
                //     if (string.IsNullOrEmpty(tokenExists))
                //     {
                //         throw new SecurityTokenException("Invalid token. No matching token found in cache.");
                //     }
                // },
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception is SecurityTokenExpiredException)
                    {
                        context.Response.Headers.Append("Token-Expired", "true");
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return context.Response.WriteAsync("Token is expired");
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return context.Response.WriteAsync("Token invalid.");
                    }
                }
            };
        });

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = singingKey,
            ValidateIssuer = false,
            ValidIssuer = jwtToken.Issuer,
            ValidateAudience = false,
            ValidAudience = jwtToken.Audience,
            RequireExpirationTime = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        services.AddSingleton(tokenValidationParameters);
        

        return services;
    }

    private static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtConfiguration>(configuration.GetSection("JwtConfiguration"));
        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = false;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
            options.Lockout.MaxFailedAccessAttempts = 3;
            options.Lockout.AllowedForNewUsers = true;

            options.User.RequireUniqueEmail = true;

            options.SignIn.RequireConfirmedEmail = true;
        });

        services.AddJwtTokenService(configuration);
        services.AddAuthorization();

        return services;
    }


    private static IServiceCollection AddKafkaService(this IServiceCollection services, IConfiguration configuration)
    {

        services.Configure<KafkaSettings>(configuration.GetSection("Kafka"));
        services.AddSingleton<IKafkaProducerService<string, string>, KafkaProducerService<string, string>>();
        
        return services;
    }


 

    
    public static WebApplication UseInfrastructureService(this WebApplication app)
    {
        return app;
    }
}