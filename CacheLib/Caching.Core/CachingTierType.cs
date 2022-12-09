using Medrio.Caching.Abstraction.CachingProviders;

namespace Medrio.Caching.Abstraction
{
 
    public enum CachingTierType: short
    {
        [CacheProvider(typeof(IInMemoryCacheProvider))]
        LocalInMemory = 1,

        [CacheProvider(typeof(IDistributedCacheProvider))]
        Distributed = 2
    }
}
