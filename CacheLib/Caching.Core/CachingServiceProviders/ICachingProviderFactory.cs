
namespace Medrio.Caching.Abstraction.CachingServiceProviders
{
    public interface ICachingProviderFactory
    {
        ICachingServiceProvide GetCachingServiceProvide(CachingTierType cachingTierType);
    }
}
