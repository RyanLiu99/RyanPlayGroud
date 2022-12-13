using Medrio.Caching.Dependencies;
using Medrio.Infrastructure.Ioc.Dependency;
using Microsoft.Extensions.DependencyInjection;

namespace Medrio.Caching.InMemoryInvalidation.DataChannel
{
    [RegisterAs(typeof(IDependenciesDataChannel), Lifetime = ServiceLifetime.Singleton)]
    internal class DependenciesDataChannel : BucketDataChannel<CachingDependencies>, IDependenciesDataChannel
    {
    }
}
