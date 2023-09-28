using System.Timers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Timer = System.Timers.Timer;

using KafkaCommon.Generated;
using KafkaCommon.Services;
using KafkaCommon.Models;

using Bogus;

namespace KafkaProducer;

public interface IMainService : ISingletonService { }

public class MainService : IMainService
{

    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;
    private readonly AppOptions _options;
    private readonly ProducerService _producerService;
    private readonly Faker<SensorData> _sensorDataFaker;
    private readonly Faker<InferredSensorData> _inferredSensorDataFaker;
    private readonly Timer _producerTimer;
    public MainService(IProducerService producerService, IOptions<AppOptions> options, ILogger<MainService> logger)
    {
        _logger = logger;
        _producerService = (ProducerService)producerService;
        _options = options.Value;
        _sensorDataFaker = new Faker<SensorData>()
            .RuleFor(s => s.Id, f => f.Random.Guid())
            .RuleFor(s => s.TimeStamp, f => f.Date.Recent())
            .RuleFor(s => s.Temperature, f => Math.Round(f.Random.Decimal(-81, 75), 2))
            .RuleFor(s => s.Humidity, f => Math.Round(f.Random.Decimal(0, 100), 2))
            .RuleFor(s => s.BatteryLevel, f => f.Random.Int(0, 100))
            .RuleFor(s => s.ExtraPayload, f => f.Random.Bytes(64));
        _inferredSensorDataFaker = new Faker<InferredSensorData>()
            .RuleFor(s => s.Id, f => f.Random.Guid())
            .RuleFor(s => s.TimeStamp, f => f.Date.Recent())
            .RuleFor(s => s.Temperature, f => Math.Round(f.Random.Decimal(-81, 75), 2))
            .RuleFor(s => s.Humidity, f => Math.Round(f.Random.Decimal(0, 100), 2))
            .RuleFor(s => s.BatteryLevel, f => f.Random.Int(0, 100))
            .RuleFor(s => s.ExtraPayload, f => f.Random.Bytes(64));
        _producerTimer = new Timer();
        _producerTimer.Interval = options.Value.MessageProductionIntervalMs;
        _producerTimer.Elapsed += ProducerTimerElapsedHandler;
        _producerTimer.Start();
        _logger.LogInformation("MainService initialized");
    }

    public async void ProducerTimerElapsedHandler(object? sender, ElapsedEventArgs e){
        await ProduceMessageAsync();
    }

    public void StampSensorData(ref InferredSensorData data){
        data.TimeStamp = DateTime.UtcNow;
    }

    public async Task ProduceMessageAsync()
    {
        InferredSensorData sensorData = _inferredSensorDataFaker.Generate();
        StampSensorData(ref sensorData);
        await _producerService.ProduceAsync(sensorData);
    }
}