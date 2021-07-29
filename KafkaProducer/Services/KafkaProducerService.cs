using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using Confluent.Kafka;

using KafkaProducer.Configuration;

namespace KafkaProducer.Services
{
    public interface IKafkaProducerService { }
    public class KafkaProducerService : IKafkaProducerService
    {
        private Random Random { get; set; }
        // private ProducerConfig ProducerConfig {get;set;}
        private IProducer<Null, string> Producer { get; set; }
        private string Message { get; set; }
        private string Topic { get; set; }

        public int MaxParallelProductionThreads { get; set; }
        private readonly ILogger<KafkaProducerService> _logger;
        public IProducer<Null, string> BuildProducer(AppSettings appSettings, KafkaSettings kafkaSettings) {
            string bootstrapServers = kafkaSettings.BootstrapServers;
            ProducerConfig prodConf = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                ClientId = Dns.GetHostName(),
                LingerMs = 200,
                BatchSize = 327680,
                CompressionType = CompressionType.Lz4,
            };
            Topic = kafkaSettings.TopicName;
            _logger.LogInformation($"Created producer with bootstrapservers: {bootstrapServers} and topic {Topic}");
            return new ProducerBuilder<Null, string>(prodConf).Build();

        }
        public KafkaProducerService(AppSettings appSettings, KafkaSettings kafkaSettings, ILogger<KafkaProducerService> logger)
        {
            _logger = logger;
            Producer = BuildProducer(appSettings, kafkaSettings);
            MaxParallelProductionThreads = kafkaSettings.MaxParallelProductionThreads;
        }

        public async Task ProduceAsync(string message)
        {
            await Producer.ProduceAsync(Topic, new Message<Null, string> { Value = message });
        }
    }
}