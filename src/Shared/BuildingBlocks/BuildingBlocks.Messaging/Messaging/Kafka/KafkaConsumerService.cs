// using Confluent.Kafka;
// using MassTransit;
// using Microsoft.Extensions.Logging;
// using Microsoft.Extensions.Options;
//
// namespace BuildingBlocks.Messaging.Messaging.Kafka;
//
// public class KafkaConsumerService<TKey, TValue> : IKafkaConsumerService<TKey, TValue>, IDisposable
// {
//     private readonly IConsumer<TKey, TValue> _consumer;
//     private readonly KafkaSettings _kafkaSettings;
//     private readonly ILogger<KafkaConsumerService<TKey, TValue>> _logger;
//     
//     public KafkaConsumerService(IOptions<KafkaSettings> options, ILogger<KafkaConsumerService<TKey, TValue>> logger)
//     {
//         _logger = logger;
//         _kafkaSettings = options.Value;
//         var config = new ConsumerConfig
//         {
//             BootstrapServers = _kafkaSettings.BootstrapServers,
//             GroupId = _kafkaSettings.GroupId,
//             AutoOffsetReset = AutoOffsetReset.Earliest
//         };
//         _consumer = new ConsumerBuilder<TKey, TValue>(config).Build();
//     }
//     
//     public void Subscribe(string topic)
//     {
//         _consumer.Subscribe(topic);
//     }
//
//     public async Task ConsumeAsync(Func<TKey, TValue, Task> handleMessage)
//     {
//         while (true)
//         {
//             
//             
//             var consumerResult = _consumer.Consume();
//             _logger.LogInformation($"Consumer -------------- {consumerResult.Message.Key}");
//             if (consumerResult != null)
//             {
//                 await handleMessage(consumerResult.Message.Key, consumerResult.Message.Value);
//             }
//         }
//     }
//
//     public void Dispose()
//     {
//         _consumer.Close();
//         _consumer.Dispose();
//     }
// }

using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Messaging.Kafka
{
    public class KafkaConsumerService<TKey, TValue> : IKafkaConsumerService<TKey, TValue>, IDisposable
    {
        private readonly IConsumer<TKey, TValue> _consumer;
        private readonly KafkaSettings _kafkaSettings;
        private readonly ILogger<KafkaConsumerService<TKey, TValue>> _logger;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IKafkaProducerService<TKey, TValue> _producer;
        
        public KafkaConsumerService(IOptions<KafkaSettings> options, ILogger<KafkaConsumerService<TKey, TValue>> logger, IKafkaProducerService<TKey, TValue> producer)
        {
            _logger = logger;
            _producer = producer;
            _kafkaSettings = options.Value;
            _cancellationTokenSource = new CancellationTokenSource();
            var config = new ConsumerConfig
            {
                BootstrapServers = _kafkaSettings.BootstrapServers,
                GroupId = _kafkaSettings.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false, // Disable auto commit to manage offsets manually
                MaxPollIntervalMs = 300000, // Timeout for fetching messages
                FetchMaxBytes = 1048576, // Maximum bytes to fetch per poll (1MB)
                SessionTimeoutMs = 10000, // Timeout for session
            };
            _consumer = new ConsumerBuilder<TKey, TValue>(config).Build();
        }

        public void Subscribe(string topic)
        {
            _consumer.Subscribe(topic);
        }

        public async Task ConsumeAsync(Func<TKey, TValue, Task> handleMessage)
        {
            try
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    try
                    {
                        var consumerResult = _consumer.Consume(_cancellationTokenSource.Token);
                        if (consumerResult != null)
                        {
                            _logger.LogInformation($"Consumed message: {consumerResult.Message.Key}");
                            await handleMessage(consumerResult.Message.Key, consumerResult.Message.Value);
                            _consumer.Commit(consumerResult);
                        }
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError($"Error consuming message: {ex.Error.Reason}");
                        await HandleConsumeErrorAsync(ex);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Consuming operation was cancelled.");
            }
            finally
            {
                _consumer.Close();
            }
        }

        public async Task ConsumeBatchAsync(Func<IEnumerable<ConsumeResult<TKey, TValue>>, Task> handleBatchMessage)
        {
            try
            {
                var batchSize = 10;
                var batch = new List<ConsumeResult<TKey, TValue>>();

                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    try
                    {
                        var consumerResult = _consumer.Consume(_cancellationTokenSource.Token);
                        if (consumerResult != null)
                        {
                            batch.Add(consumerResult);
                            _logger.LogInformation($"Consumed message in batch: {consumerResult.Message.Key}");

                            // Process batch when batch size is reached
                            if (batch.Count >= batchSize)
                            {
                                await handleBatchMessage(batch);
                                foreach (var result in batch)
                                {
                                    _consumer.Commit(result); // Commit offsets for the whole batch
                                }
                                batch.Clear(); // Clear batch after processing
                            }
                        }
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError($"Error consuming message: {ex.Error.Reason}");
                        await HandleConsumeErrorAsync(ex);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Consuming operation was cancelled.");
            }
            finally
            {
                _consumer.Close();
            }
        }

        private async Task HandleConsumeErrorAsync(ConsumeException ex)
        {
            
            var errorMessage = new
            {
                ErrorMessage = ex.Error.Reason,
                Timestamp = DateTime.UtcNow,
            };

            string errorPayload = JsonConvert.SerializeObject(errorMessage);
            await SendToErrorTopicAsync(ex.Message, errorPayload);
        }

        private async Task SendToErrorTopicAsync(string errorDetails, string errorPayload)
        {
            var errorTopic = _kafkaSettings.ErrorTopic;

            try
            {
                TKey errorKey = default;
                
                await _producer.ProduceAsync(errorTopic, errorKey, (TValue)(object)errorPayload);

                _logger.LogWarning($"Message sent to error topic: {errorTopic}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send error message to error topic: {ex.Message}");
            }
        }


        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _consumer.Close();
            _consumer.Dispose();
            _cancellationTokenSource.Dispose();
        }
    }
}
