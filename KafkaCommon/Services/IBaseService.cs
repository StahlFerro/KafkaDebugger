using Microsoft.Extensions.DependencyInjection;

namespace KafkaCommon.Services;
public interface IBaseService
{
    ServiceLifetime Lifetime { get; }
}