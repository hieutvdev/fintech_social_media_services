using BuildingBlocks.Messaging.Messaging.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Article.Infrastructure.Services;

public class ConsumerBackgroundService : BackgroundService
{
    private readonly IKafkaConsumerService<string, string> _consumer;
    private readonly ILogger<ConsumerBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ConsumerBackgroundService(IKafkaConsumerService<string, string> consumer, IServiceProvider serviceProvider,
        ILogger<ConsumerBackgroundService> logger)
    {
        _consumer = consumer;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Kafka Consumer Background Service is stopping.");
        return base.StopAsync(cancellationToken);
    }
    
    
}