using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Confluent.Kafka;

using KafkaProducer.Configuration;

namespace KafkaProducer.Services
{
    public interface IKafkaProducerService { }
    public class KafkaProducerService : IKafkaProducerService
    {
        private Random Random { get; set; }
        private ProducerConfig ProducerConfig { get; set; }
        private IProducer<Null, string> Producer { get; set; }
        private string Message { get; set; }
        private string Topic { get; set; }
        public KafkaProducerService(AppSettings appSettings, KafkaSettings kafkaSettings)
        {
            string bootstrapServers = kafkaSettings.BootstrapServers;
            ProducerConfig = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                ClientId = Dns.GetHostName(),
                LingerMs = 200,
                BatchSize = 32768,
                CompressionType = CompressionType.Lz4,
            };
            Topic = kafkaSettings.TopicName;
            Producer = new ProducerBuilder<Null, string>(ProducerConfig).Build();
            Console.WriteLine($"Created producer with bootstrapservers: {bootstrapServers} and topic {Topic}");
            Random = new Random();
            Message = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/kafka_message.json"));
            Console.WriteLine($"Message size: {ASCIIEncoding.ASCII.GetByteCount(Message)} (ASCII), {ASCIIEncoding.UTF8.GetByteCount(Message)} (UTF-8)");
        }

        public async Task ProduceAsync()
        {
            // string payload = $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] {RandomString(5)}";
            List<Task> taskList = new List<Task>();
            Console.WriteLine("Producing messages...");
            foreach (int n in Enumerable.Range(1, 1000000))
            {
                taskList.Add(Producer.ProduceAsync(Topic, new Message<Null, string> { Value = Message }));
                // Console.WriteLine($"On topic: {Topic}, produced message: {n}");
            }
            await Task.WhenAll(taskList);
        }

        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}