
namespace Medrio.Caching.Abstraction.CachingProviders
{
    public interface ICachingProviderFactory
    {
        ICachingProvide GetCachingServiceProvide(CachingTierType cachingTierType);
    }
}
