using System;

namespace KafkaProducer.Configuration
{
    public class KafkaSettings : IJsonConfiguration
    {
        public string BootstrapServers { get; set; }
        public string TopicName { get; set; }

        public int MaxParallelProductionThreads { get; set; }
    }
}
