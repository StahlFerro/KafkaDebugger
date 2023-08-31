using Confluent.Kafka;

namespace KafkaProducer;

public class AppOptions
{
    public string Brokers { get; set; } = string.Empty;
    public string DebugTopic { get; set; } = string.Empty;
    public int MessageProductionIntervalMs { get; set; }
    public int TotalMessageCount { get; set; }
    public CompressionType MessageCompressionType { get; set; }
    public int ProducerLingerMs { get; set; }
    public int ProducerBatchSize { get; set; }
    public Acks ProducerAcks { get; set; }
    public string SensorDataSchemaPath { get; set; } = string.Empty;
}