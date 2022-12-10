using System;
using System.Collections.Generic;
using Medrio.Infrastructure.Ioc;
using Medrio.Infrastructure.Ioc.Dependency;

namespace Medrio.Caching.Abstraction.Caches
{
    [RegisterAs(typeof(ICacheFactory))]
    internal class CacheFactory : ICacheFactory
    {
        //TODO: Build map from enum CacheProviderAttribute
        private static readonly IReadOnlyDictionary<CachingTierType, Type> Map = new Dictionary<CachingTierType, Type>()
        {
            {CachingTierType.LocalInMemory, typeof(IInMemoryCache)},
            {CachingTierType.Distributed, typeof(IDistributedCache)}
        };

        public ICache GetCachingServiceProvide(CachingTierType cachingTierType)
        {
            if (!Map.TryGetValue(cachingTierType, out Type providerType))
            {
                throw new CachingSettingException($"Cannot find caching provider for {cachingTierType}");
            }

            ICache provider = IocAdapter.ResolveInContainer(providerType, true) as ICache 
                                              ?? throw new CachingSettingException($"Cannot find implementation for {providerType.FullName}");
            return provider;
        }
    }
}
