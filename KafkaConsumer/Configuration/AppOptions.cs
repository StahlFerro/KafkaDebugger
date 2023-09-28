using Confluent.Kafka;

namespace KafkaConsumer;

public class AppOptions
{
    public string Brokers { get; set; } = string.Empty;
    public string DebugTopic { get; set; } = string.Empty;
    public string ConsumerGroup { get; set; } = string.Empty;
    public int ConsumerFetchMinBytes { get; set; }
    public bool ConsumerEnableAutoCommit { get; set; }
    public AutoOffsetReset ConsumerAutoOffsetReset { get; set; }
    public string SensorDataSchemaPath { get; set; } = string.Empty;
}