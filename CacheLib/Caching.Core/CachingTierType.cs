using Medrio.Caching.Abstraction.CachingServiceProviders;

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
