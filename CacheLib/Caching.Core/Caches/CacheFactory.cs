using System;
using System.Collections.Generic;
using Medrio.Infrastructure.Ioc;
using Medrio.Infrastructure.Ioc.Dependency;

namespace Medrio.Caching.Abstraction.Caches
{
    [RegisterAs(typeof(ICacheFactory))]
    internal class CacheFactory : ICacheFactory
    {
        //TODO: Build map from enum CacheAttribute
        private static readonly IReadOnlyDictionary<CachingTierType, Type> Map = new Dictionary<CachingTierType, Type>()
        {
            {CachingTierType.LocalInMemory, typeof(IInMemoryCache)},
            {CachingTierType.Distributed, typeof(IDistributedCache)}
        };

        public ICache GetCaches(CachingTierType cachingTierType)
        {
            if (!Map.TryGetValue(cachingTierType, out Type cacheType))
            {
                throw new CachingSettingException($"Cannot find cache for {cachingTierType}");
            }

            ICache cache = IocAdapter.ResolveInContainer(cacheType, true) as ICache 
                                              ?? throw new CachingSettingException($"Cannot find implementation for {cacheType.FullName}");
            return cache;
        }
    }
}
