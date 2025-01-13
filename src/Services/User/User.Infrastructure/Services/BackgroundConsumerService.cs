// using BuildingBlocks.Helpers;

using System.Text.Json;
using BuildingBlocks.Helpers;
using BuildingBlocks.Messaging.MessageModels.AuthService;
using BuildingBlocks.Messaging.Messaging.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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

        // Delay nhỏ để Kafka khởi động (tuỳ chọn)
        await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

        var topics = _messageTopic.GetType()
            .GetProperties()
            .Select(x => x.GetValue(_messageTopic)?.ToString())
            .Where(topic => !string.IsNullOrEmpty(topic))
            .ToArray();

        if (!topics.Any())
        {
            _logger.LogError("No Kafka topics configured in MessageTopic.");
            return;
        }

        _logger.LogInformation($"Subscribing to topics: {string.Join(", ", topics)}");

        try
        {
            foreach (var topic in topics)
            {
                _consumer.Subscribe(topic);
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                await _consumer.ConsumeAsync(async (key, value) =>
                {
                    if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
                    {
                        _logger.LogWarning("Received an empty message or key from Kafka.");
                        return;
                    }

                    _logger.LogInformation($"Received message from Kafka. Topic: {key}, Message: {value}");
                    await HandleMessage(key, value);
                });
            }
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

    private async Task HandleMessage(string topic, string message)
    {
        try
        {
            switch (topic)
            {
                case var t when t == _messageTopic.AuthRegisterUserInfoTopic:
                    _logger.LogInformation($"Handling message for topic {topic}");

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var userInfoService = scope.ServiceProvider.GetRequiredService<IUserInfoRepository>();
                        string mes = JsonConvert.DeserializeObject<string>(message)!;
                        _logger.LogInformation($"Parsing message for topic {mes}");
                        if (!DataConvertHelper.TryParseObject<AuthRegisterRequestDto>(mes, out var parsedMessage))
                        {
                            _logger.LogWarning($"Failed to parse message for topic {topic}: {mes}");
                            return;
                        }
                        // var parsedMessage = JsonConvert.DeserializeObject<AuthRegisterRequestDto>(message);
                
                        var result = await userInfoService.CreateFromAuthRegisterAsync(parsedMessage);

                        if (result)
                        {
                            _logger.LogInformation($"Successfully processed message for topic {topic}");
                        }
                        else
                        {
                            _logger.LogWarning($"Failed to process message for topic {topic}");
                        }
                    }
                    break;

                default:
                    _logger.LogWarning($"Unhandled topic: {topic}");
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while handling message for topic {topic}");
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("BackgroundConsumerService is stopping.");

        await base.StopAsync(cancellationToken);
    }
}
