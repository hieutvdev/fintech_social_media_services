using System.Reflection;
using System.Text;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.Middleware;
using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.FileProviders;
using Microsoft.FeatureManagement;
using Microsoft.IdentityModel.Tokens;
using Upload.API.Configurations;
using Upload.API.Services.S3Upload;
using Upload.API.Services.Upload;
using Upload.API.Services.UploadCloudinary;

namespace Upload.API.DependencyInjection.Extensions;

public static class ServiceCollection
{
    private const string CloudinaryKey = "CloudinarySetting";

    public static IServiceCollection AddServiceConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddCarter();
        services.AddConfigurationMediaR(configuration);
        
        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddSingleton<IUploadService, UploadService>();
        services.Configure<CloudinaryConfigurationSetting>(configuration.GetSection(CloudinaryKey));
        services.AddSingleton<ICloudinaryService, CloudinaryService>();
        services.AddConfigurationS3(configuration);
        services.AddAuthenticationService(configuration);
        return services;
    }

    private static IServiceCollection AddConfigurationMediaR(this IServiceCollection services,
        IConfiguration configuration)
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

    private static IServiceCollection AddConfigurationS3(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAWSService<IAmazonS3>(new AWSOptions()
        {
            Credentials = new BasicAWSCredentials(
                configuration["S3:AccessKey"], 
                configuration["S3:SecretKey"]
                ),
            Region = Amazon.RegionEndpoint.GetBySystemName(configuration["S3:Region"])
        });

        services.AddScoped<IS3UploadService, S3UploadService>();
        return services;
    }


    private static IServiceCollection AddAuthenticationService(this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection("ApiSettings:JwtOptions");
        var secret = jwtOptions["Secret"]!;
        var audience = jwtOptions["Audience"]!;
        var issuer = jwtOptions["Issuer"]!;

        var key = Encoding.UTF8.GetBytes(secret);
        
        
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidIssuer = issuer,
                ClockSkew = TimeSpan.Zero
            };
        });
        return services;
    }

    public static WebApplication UseServiceConfiguration(this WebApplication app)
    {
        app.UseMiddleware<IpLoggingMiddleware>();

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });
        app.UseStaticFiles(options: new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "FileUploads")),
            RequestPath = "/file-uploads"
        });
        app.MapCarter();
        app.UseExceptionHandler(config => { });

        return app;
    }
}