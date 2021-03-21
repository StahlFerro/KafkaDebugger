using System;
using System.Linq;
using System.Collections.Generic;
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
            
            IServiceCollection services = new ServiceCollection();

            Type interfaceConfigType = typeof(IJsonConfiguration);

            IEnumerable<Type> concreteConfigTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => interfaceConfigType.IsAssignableFrom(p) && !p.IsInterface);

            foreach(Type configType in concreteConfigTypes) {
                Console.WriteLine(configType.Name);
                object config = Activator.CreateInstance(configType);
                ConfigurationBinder.Bind(configuration, configType.Name, config);
                services.AddSingleton(configType, config);
            }

            // IEnumerable<IJsonConfiguration> settings = concreteConfigTypes.Select(ct => (IJsonConfiguration)Activator.CreateInstance(ct));
            
            // Console.WriteLine(string.Join(", ", settings.Select(ct => string.Join(", ",settings.GetType().GetProperties().Select(m => m.Name)))));

            // var appSettings = new AppSettings();
            // var kafkaSettings = new KafkaSettings();
            // ConfigurationBinder.Bind(configuration, "AppSettings", appSettings);
            // ConfigurationBinder.Bind(configuration, "KafkaSettings", kafkaSettings);

            // services.AddSingleton(appSettings);
            // services.AddSingleton(kafkaSettings);

            services.AddSingleton<IKafkaProducerService, KafkaProducerService>();
            ServiceProvider provider = services.BuildServiceProvider();
            return provider;
        }
    }
}