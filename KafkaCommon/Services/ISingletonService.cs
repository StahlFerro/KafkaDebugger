using Microsoft.Extensions.DependencyInjection;

namespace KafkaCommon.Services;

public interface ISingletonService : IBaseService
{
    ServiceLifetime IBaseService.Lifetime => ServiceLifetime.Singleton;
}