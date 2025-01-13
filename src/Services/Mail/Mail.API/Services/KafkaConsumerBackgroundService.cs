using BuildingBlocks.Messaging.Messaging.Kafka;
using Mail.API.Interfaces;

namespace Mail.API.Services;

public class KafkaConsumerBackgroundService : BackgroundService
{
    private readonly IKafkaConsumerService<string, string> _consumer;
    private readonly ILogger<KafkaConsumerBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public KafkaConsumerBackgroundService(
        IKafkaConsumerService<string, string> consumer,
        IServiceProvider serviceProvider,
        ILogger<KafkaConsumerBackgroundService> logger)
    {
        _consumer = consumer;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Kafka Consumer Background Service is starting.");
        await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
       
        _consumer.Subscribe("verify-account");
        _logger.LogInformation("Kafka Consumer Background Service is starting. 11111111");


        try
        {
            await _consumer.ConsumeAsync(async (key, value) =>
            {
                _logger.LogInformation($"Received message from Kafka: {value}");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var sendMailService = scope.ServiceProvider.GetRequiredService<ISendMailService>();
                    await sendMailService.SendMailConfirmAccountAsync(value, stoppingToken);
                }
            });
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("BackgroundConsumerService is stopping due to cancellation.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while consuming messages.");
        }

        _logger.LogInformation("Kafka Consumer Background Service is stopping.");
    }
    
    

    public override Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Kafka Consumer Background Service is stopping.");
        return base.StopAsync(stoppingToken);
    }
}