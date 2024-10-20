using BuildingBlocks.Middleware;
using Carter;

namespace Mail.API.DependencyInjection.Extensions;

public static class WebApplicationConfiguration
{
    public static WebApplication UseService(this WebApplication app)
    {
        app.UseMiddleware<IpLoggingMiddleware>();
        app.MapCarter();
        app.UseExceptionHandler(configs => { });
        return app;
    }
}
