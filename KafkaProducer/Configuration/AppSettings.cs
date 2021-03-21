using System;

namespace KafkaProducer.Configuration {
    public class AppSettings : IJsonConfiguration
    {
        public string Version { get; set; }
    }
}
