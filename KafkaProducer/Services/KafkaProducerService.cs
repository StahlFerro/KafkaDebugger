using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace KafkaDebugger.Services
{
    public interface IKafkaProducerService {}
    public class KafkaProducerService : IKafkaProducerService {
        private Random Random { get; set; }
        private ProducerConfig ProducerConfig { get; set; }
        private IProducer<Null, string> Producer { get; set; }
        private string Topic { get; set; }
        public KafkaProducerService(string bootstrapServers, string topic) {
            ProducerConfig = new ProducerConfig {
                BootstrapServers = bootstrapServers,
                ClientId = Dns.GetHostName(),
            };
            Producer = new ProducerBuilder<Null, string>(ProducerConfig).Build();
            Random = new Random();
            Topic = topic;
        }

        public async Task ProduceAsync() {
            string payload = $"[{DateTime.Now.ToString("yyyy-mm-dd HH:MM:ss")}] {RandomString(5)}";
            await Producer.ProduceAsync(Topic, new Message<Null, string> { Value = payload });
            Console.WriteLine($"On topic: {Topic}, produced message: {payload}");
        }

        public string RandomString(int length) {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}
