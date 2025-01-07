using BuildingBlocks.Helpers;
using BuildingBlocks.Messaging.MessageModels.AuthService;
using BuildingBlocks.Messaging.Messaging.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using User.Application.Repositories;
using User.Infrastructure.Configuration;

namespace User.Infrastructure.Services;

public class BackgroundConsumerService : BackgroundService
{
    private readonly IKafkaConsumerService<string, string> _consumer;
    private readonly ILogger<BackgroundConsumerService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly MessageTopic _messageTopic;

    public BackgroundConsumerService(
        IKafkaConsumerService<string, string> consumer,
        IServiceProvider serviceProvider,
        ILogger<BackgroundConsumerService> logger,
        IOptions<MessageTopic> options)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _messageTopic = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("BackgroundConsumerService is starting.");
        await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        try
        {
            
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("BackgroundConsumerService is stopping due to cancellation.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while consuming messages.");
        }
    }

    private void HandleMessage(string topic, string message)
    {
        try
        {
            switch (topic)
            {
                case var t when t == _messageTopic.AuthRegisterUserInfoTopic:
                    _logger.LogInformation($"Handling message for {topic}: {message}");
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var userInfoService = scope.ServiceProvider.GetRequiredService<IUserInfoRepository>();
                        if (!DataConvertHelper.TryParseObject<AuthRegisterRequestDto>(message, out AuthRegisterRequestDto val))
                        {
                            _logger.LogWarning($"Failed to parse message for topic {topic}.");
                            return;
                        }
                        
                    }
                    break;

                default:
                    _logger.LogWarning($"Received message for unhandled topic: {topic}");
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while handling message for topic {topic}.");
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("BackgroundConsumerService is stopping.");
        return base.StopAsync(cancellationToken);
    }
}
