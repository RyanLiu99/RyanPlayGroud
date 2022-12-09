using System;
using System.Collections.Generic;
using Medrio.Infrastructure.Ioc;
using Medrio.Infrastructure.Ioc.Dependency;

namespace Medrio.Caching.Abstraction.CachingServiceProviders
{
    [RegisterAs(typeof(ICachingProviderFactory))]
    internal class CachingProviderFactory : ICachingProviderFactory
    {
        //TODO: Build map from enum CacheProviderAttribute
        private static readonly IReadOnlyDictionary<CachingTierType, Type> Map = new Dictionary<CachingTierType, Type>()
        {
            {CachingTierType.LocalInMemory, typeof(IInMemoryCacheProvider)},
            {CachingTierType.Distributed, typeof(IDistributedCacheProvider)}
        };

        public ICachingProvide GetCachingServiceProvide(CachingTierType cachingTierType)
        {
            if (!Map.TryGetValue(cachingTierType, out Type providerType))
            {
                throw new CachingSettingException($"Cannot find caching provider for {cachingTierType}");
            }

            ICachingProvide provider = IocAdapter.ResolveInContainer(providerType, true) as ICachingProvide 
                                              ?? throw new CachingSettingException($"Cannot find implementation for {providerType.FullName}");
            return provider;
        }
    }
}
