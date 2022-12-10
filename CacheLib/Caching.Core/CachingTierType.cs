using Medrio.Caching.Abstraction.Caches;

namespace Medrio.Caching.Abstraction
{
 
    public enum CachingTierType: byte
    {
        [Cache(typeof(IInMemoryCache))]
        LocalInMemory = 1,

        [Cache(typeof(IDistributedCache))]
        Distributed = 2
    }
}
