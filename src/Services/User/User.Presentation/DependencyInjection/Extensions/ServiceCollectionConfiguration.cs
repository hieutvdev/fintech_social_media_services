using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace User.Presentation.DependencyInjection.Extensions;

public static class ServiceCollectionConfiguration
{
    public static IServiceCollection AddPresentationService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCarter();
        services.AddApiVersioningService();
        return services;
    }
    
    private static IServiceCollection AddApiVersioningService(this IServiceCollection services)
    {
        services.AddApiVersioning(options => options.ReportApiVersions = true)
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        return services;
    }
    private static void ConfigureSwagger(this WebApplication app)
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
    }
    
    
    public static WebApplication UsePresentationService(this WebApplication app)
    {
        
        app.MapCarter();
        
        app.ConfigureSwagger();
        
        return app;
    }
    
    
}