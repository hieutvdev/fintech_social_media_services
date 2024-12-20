using Carter;
using Serilog;

namespace Mail.API.Installers;

public class SerilogInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
        services.AddCarter();
    }
}