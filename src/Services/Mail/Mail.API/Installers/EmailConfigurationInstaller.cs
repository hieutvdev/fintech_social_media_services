using Mail.API.Configurations;

namespace Mail.API.Installers;

public class EmailConfigurationInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
    }
}