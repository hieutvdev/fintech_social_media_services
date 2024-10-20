
using Mail.API.Interfaces;
using Mail.API.Services;

namespace Mail.API.Installers;

public class ServiceInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ISendMailService, SendMailService>();
    }
}
