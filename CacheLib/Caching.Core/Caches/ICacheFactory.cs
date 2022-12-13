
namespace Medrio.Caching.Abstraction.Caches
{
    public interface ICacheFactory
    {
        ICache GetCache(CachingTierType cachingTierType);
    }
}
