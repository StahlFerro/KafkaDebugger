using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using KafkaDebugger.Services;

namespace KafkaDebugger
{
    public class Program
    {
        public static void Main(string[] args) => new Program().StartAsync(args).GetAwaiter().GetResult();

        public async Task StartAsync(string[] args) {
            ServiceProvider provider = ConfigureServices(args);
            KafkaProducerService kafkaProducerService = provider.GetService<KafkaProducerService>();
            while (true) {
                await kafkaProducerService.ProduceAsync();
                await Task.Delay(1000);
            }
            await Task.Delay(-1);
        }

        public ServiceProvider ConfigureServices(string[] args) {
            KafkaProducerService kafkaProducerService = new KafkaProducerService(args[0], args[1]);
            IServiceCollection services = new ServiceCollection().AddSingleton(kafkaProducerService);
            ServiceProvider provider = services.BuildServiceProvider();
            return provider;
        }
    }
}
