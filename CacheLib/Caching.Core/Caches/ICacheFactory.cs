
namespace Medrio.Caching.Abstraction.Caches
{
    public interface ICacheFactory
    {
        ICache GetCaches(CachingTierType cachingTierType);
    }
}
