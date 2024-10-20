
using BuildingBlocks.Exceptions.Handler;
using FluentValidation;
using Microsoft.FeatureManagement;
using System.Reflection;

namespace Mail.API.Installers;

public class MediarInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {

        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddMediatR(configs =>
        {
            configs.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            configs.AddOpenBehavior(typeof(ValidationBehavior<,>));
            configs.AddOpenBehavior(typeof(PerformancePipelineBehavior<,>));
        });

        services.AddFeatureManagement();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
