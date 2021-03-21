using System;

namespace KafkaProducer.Configuration
{
    public class KafkaSettings
    {
        public string BootstrapServers { get; set; }
        public string TopicName { get; set; }
    }
}
