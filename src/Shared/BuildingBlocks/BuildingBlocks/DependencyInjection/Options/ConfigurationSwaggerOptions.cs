using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BuildingBlocks.DependencyInjection.Options;

public class ConfigurationSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;
    public ConfigurationSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach(var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName,
                new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = AppDomain.CurrentDomain.FriendlyName,
                    Version = description.ApiVersion.ToString(),
                });
        }

        options.MapType<DateOnly>(() => new OpenApiSchema
        {
            Type = "string",
            Format = "date",
            Example = new OpenApiString(DateTime.MinValue.ToString())
        });

        options.CustomOperationIds(apiDes =>
        {
            return apiDes.ActionDescriptor.RouteValues["controller"]
                   + "_" + apiDes.HttpMethod
                   + "_" + string.Join("_", apiDes.ParameterDescriptions.Select(p => p.Name));
        });
    }
}