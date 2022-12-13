using System;
using Medrio.Caching.Dependencies;
using System.Threading.Tasks;
using Medrio.Caching.InMemoryInvalidationService;
using Medrio.Infrastructure.Ioc.Dependency;

namespace Medrio.Caching.InMemoryInvalidation
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
