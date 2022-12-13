using System;
using System.Threading.Tasks;
using Medrio.Caching.Dependencies;
using Medrio.Infrastructure.Ioc.Dependency;

namespace Medrio.Caching.InMemoryInvalidationService
{
    [RegisterAs(typeof(IMemoryCacheInvalidator))]
    internal class MemoryCacheInvalidator : IMemoryCacheInvalidator
    {
        public Task InvalidateCache(CachingDependencies dependencies)
        {
            throw new NotImplementedException();
        }
    }
}
