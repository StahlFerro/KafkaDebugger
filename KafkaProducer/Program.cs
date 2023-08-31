using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace KafkaProducer;

public class Program
{

    public static async Task Main(string[] args)
    {
        var hostBuilder = Host.CreateApplicationBuilder(args);

        AddConfiguration(ref hostBuilder, args);
        AddLogging(ref hostBuilder);
        AddOptions(ref hostBuilder);
        AddServices(ref hostBuilder);

        var host = hostBuilder.Build();
        EvaluateServices(host.Services);

        await host.RunAsync();
    }

    public static void AddConfiguration(ref HostApplicationBuilder hostBuilder, string[] args)
    {
        hostBuilder.Configuration.Sources.Clear();
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .AddJsonFile("./appsettings.json")
            .Build();
        hostBuilder.Configuration.AddConfiguration(configuration);
    }

    public static void AddOptions(ref HostApplicationBuilder hostBuilder)
    {
        hostBuilder.Services.Configure<AppOptions>(hostBuilder.Configuration.GetSection(nameof(AppOptions)));
    }

    public static void AddServices(ref HostApplicationBuilder hostBuilder)
    {
        hostBuilder.Services.AddSingleton<IProducerService, ProducerService>();
        hostBuilder.Services.AddSingleton<IMainService, MainService>();
    }

    public static void AddLogging(ref HostApplicationBuilder hostBuilder)
    {
        hostBuilder.Logging.ClearProviders();
        var nlogOptions = new NLogProviderOptions
        {
            ReplaceLoggerFactory = true,
            AutoShutdown = true,
        };
        var config = new ConfigurationBuilder()
            // .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        hostBuilder.Logging.SetMinimumLevel(LogLevel.Trace);
        hostBuilder.Logging.AddNLog(config);
    }

    public static void EvaluateServices(IServiceProvider serviceProvider)
    {
        var mainService = (MainService)(serviceProvider.GetService<IMainService>() ?? throw new ArgumentNullException());
        // Console.WriteLine(MainService.GetSystemEval());
    }
}