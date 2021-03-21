using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using KafkaProducer.Configuration;
using KafkaProducer.Services;

namespace KafkaDebugger
{
    public class Program
    {

        public static void Main(string[] args) => new Program().MainAsync(args).GetAwaiter().GetResult();

        public async Task MainAsync(string[] args)
        {
            ServiceProvider provider = BuildServiceProvider(args);
            KafkaProducerService kafkaProducerService = (KafkaProducerService)provider.GetService<IKafkaProducerService>();
            await kafkaProducerService.ProduceAsync();
            await Task.Delay(-1);
        }

        public ServiceProvider BuildServiceProvider(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
                
            var appSettings = new AppSettings();
            var kafkaSettings = new KafkaSettings();
            ConfigurationBinder.Bind(configuration, "AppSettings", appSettings);
            ConfigurationBinder.Bind(configuration, "KafkaSettings", kafkaSettings);

            IServiceCollection services = new ServiceCollection();
            services.AddSingleton(appSettings);
            services.AddSingleton(kafkaSettings);
            services.AddSingleton<IKafkaProducerService, KafkaProducerService>();
            ServiceProvider provider = services.BuildServiceProvider();
            return provider;
        }
    }
}