
using BuildingBlocks.Exceptions.Handler;

using System.Reflection;
using BuildingBlocks.Behaviors;
using FluentValidation;
using Microsoft.FeatureManagement;

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
