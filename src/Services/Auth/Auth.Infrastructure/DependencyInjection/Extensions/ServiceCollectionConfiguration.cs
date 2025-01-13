using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
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
        var ipAddress = context.Connection.RemoteIpAddress?.ToString();
        var userAgent = context.Request.Headers["User-Agent"].ToString();
        string deviceInfo = $"{ipAddress}-{userAgent}";

        return deviceInfo.Replace(" ", string.Empty);  // Remove spaces
    }

    private static IServiceCollection AddCookiePolicyOptions(this IServiceCollection services)
    {
        services.Configure<CookiePolicyOptions>(options =>
        {
            options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always; 
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.Lax;
        });
        return services;
    }

    private static IServiceCollection AddJwtTokenService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        var jwtTokenConfig = new JwtConfiguration();
        configuration.GetSection("JwtConfiguration").Bind(jwtTokenConfig);
        var key = Encoding.UTF8.GetBytes(jwtTokenConfig.Secret);

        var signingKey = new SymmetricSecurityKey(key);
        services.AddDistributedMemoryCache(); 

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateIssuer = true,
                    ValidIssuer = jwtTokenConfig.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtTokenConfig.Audience,
                    ClockSkew = TimeSpan.Zero,
                };

                x.Events = new JwtBearerEvents
                {
                    // OnMessageReceived = context =>
                    // {
                    //     if (!context.Request.Headers.ContainsKey("Authorization"))
                    //     {
                    //         context.Response.Headers.Append("Missing Token", "true");
                    //         context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    //         return context.Response.WriteAsync("Token is expired");
                    //     }
                    //
                    //     return Task.CompletedTask;
                    // },
                    OnAuthenticationFailed = context =>
                    {
                        if (!context.Response.HasStarted)
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

                        return Task.CompletedTask;
                    }
                    
                };
            });
        
        // services.AddAuthentication(options =>
        //     {
        //         options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //         options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //         options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //     })
        //     .AddCookie()
        //     .AddFacebook(facebookOptions =>
        //     {
        //         facebookOptions.AppId = configuration["SSO:Facebook:AppId"]!;
        //         facebookOptions.AppSecret = configuration["SSO:Facebook:AppSecret"]!;
        //
        //         facebookOptions.Events = new OAuthEvents()
        //         {
        //             OnRedirectToAuthorizationEndpoint = context =>
        //             {
        //                 Console.WriteLine(context.RedirectUri);
        //                 return   Task.CompletedTask;
        //             }
        //     
        //         };
        //     })
        //     .AddGoogle(googleOptions =>
        //     {
        //         googleOptions.ClientId = configuration["SSO:Google:ClientId"]!;
        //         googleOptions.ClientSecret = configuration["SSO:Google:ClientSecret"]!;
        //       
        //         
        //         googleOptions.Events = new OAuthEvents{
        //             OnRedirectToAuthorizationEndpoint = context =>
        //             {
        //                 Console.WriteLine(context.RedirectUri);
        //                 return   Task.CompletedTask;
        //             }
        //     
        //         };
        //     });
        
        // services.AddAuthentication(options =>
        //     {
        //         options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //         options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //         options.DefaultChallengeScheme = "Discord";
        //     })
        //     .AddOAuth("Discord", options =>
        //     {
        //         options.ClientId = configuration["SSO:Discord:ClientId"];
        //         options.ClientSecret = configuration["SSO:Discord:ClientSecret"];
        //         options.CallbackPath = new PathString("/signin-discord");
        //
        //         options.AuthorizationEndpoint = "https://discord.com/api/oauth2/authorize";
        //         options.TokenEndpoint = "https://discord.com/api/oauth2/token";
        //         options.UserInformationEndpoint = "https://discord.com/api/users/@me";
        //
        //         options.SaveTokens = true;
        //         options.Scope.Add("identify");
        //         options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
        //         options.ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
        //         options.ClaimActions.MapJsonKey("urn:discord:avatar", "avatar");
        //
        //         options.Events = new OAuthEvents
        //         {
        //             OnRedirectToAuthorizationEndpoint = async context =>
        //             {
        //                 Console.WriteLine(context.RedirectUri); 
        //             },
        //             
        //             OnCreatingTicket = async context =>
        //             {
        //                 var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
        //                 request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
        //
        //                 Console.WriteLine(request.Headers.Authorization);
        //                 var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        //                 response.EnsureSuccessStatusCode();
        //
        //                 var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        //                 context.RunClaimActions(user.RootElement);
        //
        //                
        //                 var userId = user.RootElement.GetProperty("id").GetString();
        //                 var username = user.RootElement.GetProperty("username").GetString();
        //                 var email = user.RootElement.TryGetProperty("email", out var emailProp) ? emailProp.GetString() : null;
        //                 var avatar = user.RootElement.TryGetProperty("avatar", out var avatarProp) ? avatarProp.GetString() : null;
        //
        //                 // Thêm thông tin vào Claims
        //                 if (!string.IsNullOrEmpty(email))
        //                 {
        //                     context.Identity.AddClaim(new Claim(ClaimTypes.Email, email));
        //                 }
        //                 if (!string.IsNullOrEmpty(avatar))
        //                 {
        //                     context.Identity.AddClaim(new Claim("urn:discord:avatar", avatar));
        //                 }
        //             }
        //         };
        //     });
        

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