using CommunityToolkit.HighPerformance;
using Confluent.Kafka;
using Avro;
using SolTechnology.Avro;
using Avro.Reflect;
using Avro.IO;
using KafkaCommon.Generated;
using KafkaCommon.Models;

namespace KafkaCommon.Serde;

public class InferredSensorDataAvroAsyncSerializer : IAsyncSerializer<InferredSensorData>
{
    private Schema? _schema;
    private ReflectWriter<InferredSensorData> _reflectWriter;

    public InferredSensorDataAvroAsyncSerializer(string schemaFilePath)
    {
        string json = File.ReadAllText(schemaFilePath);
        _schema = Schema.Parse(json);
        _reflectWriter = new ReflectWriter<InferredSensorData>(_schema, new ClassCache());
    }

    public Task<byte[]> SerializeAsync(InferredSensorData data, SerializationContext context)
    {
        Task<byte[]> serRes = Task.Run(() =>
        {
            byte[] buf;
            using (var stream = new MemoryStream())
            {
                _reflectWriter.Write(data, new BinaryEncoder(stream));
                buf = stream.ToArray();
            }
            return buf;
        });
        return serRes;
    }
}


public class InferredSensorDataAvroDeserializer : IDeserializer<InferredSensorData>
{
    private Schema? _schema;
    private ReflectReader<InferredSensorData> _reflectReader;
    public InferredSensorDataAvroDeserializer(string schemaFilePath)
    {
        string json = File.ReadAllText(schemaFilePath);
        _schema = Schema.Parse(json);
        _reflectReader = new ReflectReader<InferredSensorData>(_schema, _schema, new ClassCache());
    }
    public InferredSensorData Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        InferredSensorData sensorData;
        using (var stream = new MemoryStream())
        {
            stream.Write(data);
            stream.Seek(0, SeekOrigin.Begin);
            sensorData = _reflectReader.Read(new BinaryDecoder(stream));
        }
        return sensorData;
    }
}

public class InferredSensorDataAvroAsyncDeserializer : IAsyncDeserializer<InferredSensorData>
{
    private Schema? _schema;
    private ReflectReader<InferredSensorData> _reflectReader;
    public InferredSensorDataAvroAsyncDeserializer(string schemaFilePath)
    {
        string json = File.ReadAllText(schemaFilePath);
        _schema = Schema.Parse(json);
        _reflectReader = new ReflectReader<InferredSensorData>(_schema, _schema, new ClassCache());
    }
    public Task<InferredSensorData> DeserializeAsync(ReadOnlyMemory<byte> data, bool isNull, SerializationContext context)
    {
        Task<InferredSensorData> derRes = Task.Run(() =>
        {
            InferredSensorData sensorData;
            using (var stream = data.AsStream())
            {
                sensorData = _reflectReader.Read(new BinaryDecoder(stream));
            }
            return sensorData;
        });
        return derRes;
    }
}


public class SensorDataAvroSolTechnologyAsyncSerializer : IAsyncSerializer<SensorData>
{
    private Schema? _schema;

    public SensorDataAvroSolTechnologyAsyncSerializer(string schemaFilePath)
    {
        string json = File.ReadAllText(schemaFilePath);
        _schema = Schema.Parse(json);
    }

    public Task<byte[]> SerializeAsync(SensorData data, SerializationContext context)
    {
        Task<byte[]> serRes = Task.Run(() =>
        {
            byte[] buf = AvroConvert.Serialize(data, CodecType.Brotli);
            return buf;
        });
        return serRes;
    }
}