
namespace Medrio.Caching.Abstraction.Caches
{
    public interface ICacheFactory
    {
        ICache GetCachingServiceProvide(CachingTierType cachingTierType);
    }
}
