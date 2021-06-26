using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using KafkaProducer.Models;

namespace KafkaProducer.Services
{
    public interface IMainService { }
    public class MainService : IMainService
    {
        private readonly ILogger<MainService> _logger;
        private KafkaProducerService KafkaProducerService { get; set; }
        public MainService(IKafkaProducerService kafkaProducerService, ILogger<MainService> logger)
        {
            KafkaProducerService = (KafkaProducerService)kafkaProducerService;
            _logger = logger;
        }

        public async Task StartProductionAsync()
        {
            int messageCount = 300000;
            SemaphoreSlim threadLimiter = new SemaphoreSlim(KafkaProducerService.MaxParallelProductionThreads, KafkaProducerService.MaxParallelProductionThreads);
            string kMessageJson = await File.ReadAllTextAsync("Data/kafka_message_10.json");

            _logger.LogInformation($"Start {string.Format("{0:#,0.####}", messageCount)} tasks with max parallel {KafkaProducerService.MaxParallelProductionThreads}...");
            List<Task> taskList = new List<Task>();
            for (int i = 0; i < messageCount; i++)
            {
                Task task = Task.Run(async () =>
                {
                    await threadLimiter.WaitAsync();
                    await KafkaProducerService.ProduceAsync(kMessageJson);
                    threadLimiter.Release();
                });
                taskList.Add(task);
            }
            _logger.LogInformation("Waiting for all tasks to complete...");
            await Task.WhenAll(taskList);
            _logger.LogInformation("All production tasks completed!");
        }
    }
}