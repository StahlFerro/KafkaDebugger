using System.Threading.Tasks;
using KafkaDebugger.Services;
using Microsoft.Extensions.DependencyInjection;

namespace KafkaDebugger
{
    public class Program
    {
        public static void Main(string[] args) => new Program().MainAsync(args).GetAwaiter().GetResult();

        public async Task MainAsync(string[] args)
        {
            ServiceProvider provider = ConfigureServices(args);
            KafkaProducerService kafkaProducerService = provider.GetService<KafkaProducerService>();
            await kafkaProducerService.ProduceAsync();
            await Task.Delay(-1);
        }

        public ServiceProvider ConfigureServices(string[] args)
        {
            KafkaProducerService kafkaProducerService = new KafkaProducerService(args[0], args[1]);
            IServiceCollection services = new ServiceCollection().AddSingleton(kafkaProducerService);
            ServiceProvider provider = services.BuildServiceProvider();
            return provider;
        }
    }
}