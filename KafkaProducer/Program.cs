using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NLog;
using NLog.Extensions.Logging;
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
            MainService mainService = (MainService)provider.GetService<IMainService>();
            await mainService.StartProductionAsync();
            await Task.Delay(-1);
        }

        public ServiceProvider BuildServiceProvider(string[] args)
        {
            IServiceCollection services = new ServiceCollection();

            IConfiguration configuration = BuildConfiguration(args);
            services = AddSettingsToService(services, configuration);

            services.AddSingleton<IKafkaProducerService, KafkaProducerService>();
            services.AddSingleton<IMainService, MainService>();
            services.AddLogging(loggingBuilder =>
            {
                // configure Logging with NLog
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                loggingBuilder.AddNLog(configuration);
            });
            ServiceProvider provider = services.BuildServiceProvider();
            return provider;
        }


        /// <summary>
        /// Build IConfiguration instance from appsettings.json and console arguments (if any)
        /// </summary>
        /// <param name="args">Console arguments (if any)</param>
        /// <returns>IConfiguration</returns>
        public IConfiguration BuildConfiguration(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
            NLog.LogManager.Configuration = new NLogLoggingConfiguration(configuration.GetSection("NLog"));
            return configuration;
        }


        /// <summary>
        /// Instantiate Settings instances that implements IJsonConfiguration, with values from IConfiguration instance, 
        /// and then injects them into the service collection
        /// </summary>
        /// <param name="services">The ServiceCollection to add the Settings dependencies as singletons</param>
        /// <param name="configuration">IConfiguration instance that is built from appsettings.json</param>
        /// <returns>IServiceCollection</returns>
        public IServiceCollection AddSettingsToService(IServiceCollection services, IConfiguration configuration)
        {
            Type interfaceConfigType = typeof(IJsonConfiguration);

            IEnumerable<Type> concreteConfigTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => interfaceConfigType.IsAssignableFrom(p) && !p.IsInterface);

            foreach (Type configType in concreteConfigTypes)
            {
                Console.WriteLine(configType.Name);
                object config = Activator.CreateInstance(configType);
                ConfigurationBinder.Bind(configuration, configType.Name, config);
                services.AddSingleton(configType, config);
            }
            return services;
        }

    }
}