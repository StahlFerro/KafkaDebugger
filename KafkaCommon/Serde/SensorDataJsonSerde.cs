using Confluent.Kafka;
using Avro.Reflect;
using KafkaCommon.Models;

namespace KafkaCommon.Serde;

public class SensorDataJsonAsyncSerializer : IAsyncSerializer<SensorData>
{
    public Task<byte[]> SerializeAsync(SensorData data, SerializationContext context)
    {
        throw new NotImplementedException();
    }
}