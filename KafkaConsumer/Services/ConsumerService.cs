using System;
using System.Text;
using System.Net;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

using KafkaCommon.Services;
using KafkaCommon.Generated;
using KafkaCommon.Serde;

using Confluent.Kafka;

namespace KafkaConsumer;

public interface IConsumerService : ISingletonService { }
public class ConsumerService : IConsumerService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;
    private readonly string _topic;
    private readonly AppOptions _options;
    private IConsumer<Null, InferredSensorData> Consumer { get; set; }
    public ConsumerService(IServiceProvider serviceProvider, IOptions<AppOptions> options, ILogger<ConsumerService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _options = options.Value;
        _topic = _options.DebugTopic;
        var config = new ConsumerConfig
        {
            BootstrapServers = _options.Brokers,
            ClientId = Dns.GetHostName(),
            AutoOffsetReset = _options.ConsumerAutoOffsetReset,
            GroupId = _options.ConsumerGroup,
            EnableAutoCommit = _options.ConsumerEnableAutoCommit,
            FetchMinBytes = _options.ConsumerFetchMinBytes,
        };
        var builder = new ConsumerBuilder<Null, InferredSensorData>(config)
            .SetValueDeserializer(new InferredSensorDataAvroDeserializer(_options.SensorDataSchemaPath));
        Consumer = builder.Build();
        _logger.LogInformation("ConsumerService initialized");
        List<string> topics = new List<string>() { _options.DebugTopic };
        Consumer.Subscribe(topics);
        ConusmerLoop();
    }

    public void ConusmerLoop()
    {
        try
        {
            while (true)
            {
                try
                {
                    var cr = Consumer.Consume();
                    _logger.LogInformation($"Consumed message '{cr.Message.Value}' at: '{cr.TopicPartitionOffset}'.");
                }
                catch (ConsumeException e)
                {
                    _logger.LogError($"ConsumeException occurred: {e.Error.Reason}");
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Close and Release all the resources held by this consumer  
            Consumer.Close();
        }
    }
}