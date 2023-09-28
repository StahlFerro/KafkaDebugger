# KafkaDebugger

.NET Test Kafka Producer and Consumer twin apps to debug/test Kafka server implementations.

Default configuration described in `appsettings_example.json` on both `KafkaProducer` and `KafkaConsumer`:

- Kafka topic: `sensor_data`
- Avro schema: `./KafkaCommon/Schemas/InferredSensorDataAvroSchema.json`
- Generated model: `./KafkaCommon/Generated/InferredSensorData.cs`

