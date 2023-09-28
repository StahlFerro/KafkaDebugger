using System;
using System.Text;
using System.Net;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

using KafkaCommon.Services;
using KafkaCommon.Generated;
using KafkaCommon.Serde;

using Confluent.Kafka;

namespace KafkaProducer;

public interface IProducerService : ISingletonService { }
public class ProducerService : IProducerService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;
    private readonly string _topic;
    private readonly AppOptions _options;
    private IProducer<Null, InferredSensorData> Producer { get; set; }
    public ProducerService(IServiceProvider serviceProvider, IOptions<AppOptions> options, ILogger<ProducerService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _options = options.Value;
        _topic = _options.DebugTopic;
        var config = new ProducerConfig
        {
            BootstrapServers = _options.Brokers,
            ClientId = Dns.GetHostName(),
            LingerMs = _options.ProducerLingerMs,
            BatchSize = _options.ProducerBatchSize,
            CompressionType = _options.MessageCompressionType,
            Acks = _options.ProducerAcks,
        };
        var builder = new ProducerBuilder<Null, InferredSensorData>(config)
            .SetValueSerializer(new InferredSensorDataAvroAsyncSerializer(_options.SensorDataSchemaPath));
        Producer = builder.Build();
        _logger.LogInformation("ProducerService initialized");
    }

    public async Task ProduceAsync(InferredSensorData data)
    {
        var msg = new Message<Null, InferredSensorData>();
        msg.Value = data;
        msg.Timestamp = new Timestamp(data.TimeStamp);
        await Producer.ProduceAsync(_topic, msg);
        _logger.LogDebug("Produced message");
    }
}