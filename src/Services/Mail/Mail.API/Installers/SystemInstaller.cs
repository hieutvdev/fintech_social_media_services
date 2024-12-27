using BuildingBlocks.Messaging.Messaging.Kafka;

namespace Mail.API.Installers;

public class SystemInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(typeof(IKafkaConsumerService<,>), typeof(KafkaConsumerService<,>));
        services.AddSingleton(typeof(IKafkaProducerService<,>), typeof(KafkaProducerService<,>));
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}