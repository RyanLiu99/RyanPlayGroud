
namespace Medrio.Caching.Abstraction.CachingServiceProviders
{
    public interface ICachingProviderFactory
    {
        ICachingProvide GetCachingServiceProvide(CachingTierType cachingTierType);
    }
}
