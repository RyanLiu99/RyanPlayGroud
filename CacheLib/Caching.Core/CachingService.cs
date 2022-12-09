using System;
using System.Linq;
using System.Threading.Tasks;
using Medrio.Caching.Abstraction.CachingProviders;
using Medrio.Caching.Abstraction.Dependencies;
using Medrio.Infrastructure.Ioc.Dependency;
using Microsoft.Extensions.DependencyInjection;

namespace Medrio.Caching.Abstraction
{
    [RegisterAs(typeof(ICachingService), Lifetime = ServiceLifetime.Transient )]
    internal class CachingService : ICachingService
    {
        private readonly ICachingProviderFactory _factory;

        public CachingService(ICachingProviderFactory factory)
        {
            _factory = factory;
        }

        public bool TryGet<T>(string key, out T? data, params CachingTierType[] tierTypes)
        {
            tierTypes.MakeSureValid();

            foreach (var tierType in tierTypes.Sort())  //try local cache first
            {
                var cachingProvider = _factory.GetCachingServiceProvide(tierType);
                var found = cachingProvider.TryGet(key, out data);
                if (found)
                {
                    return found;
                }
            }
            data = default(T?);
            return false;
        }

        public async Task<T?> GetAsync<T>(string key, params CachingTierType[] tierTypes)
        {
            tierTypes.MakeSureValid();

            foreach (var tierType in tierTypes.Sort())  //try local cache first
            {
                var cachingProvider = _factory.GetCachingServiceProvide(tierType);
                var found = await cachingProvider.TryGetAsync<T>(key, out T? data).ConfigureAwait(false);
                if (found)
                {
                    return data;
                }
            }
            return default(T?);
        }

        public void Set<T>(string key, T data, CachingTier tier, CachingDependencies? dependencies = null)
        {
            _ = tier ?? throw new ArgumentNullException(nameof(tier));
            var cachingProvider = _factory.GetCachingServiceProvide(tier.TierType);
            var cacheEntry = new CacheDataEntry<T>(data, dependencies);
            cachingProvider.Set(key, cacheEntry, tier.CacheEntryOption);
        }

        public Task SetAsync<T>(string key, T data, CachingTier tier, CachingDependencies? dependencies = null)
        {
            _ = tier ?? throw new ArgumentNullException(nameof(tier));
            var cachingProvider = _factory.GetCachingServiceProvide(tier.TierType);

            var cacheEntry = new CacheDataEntry<T>(data, dependencies);
            return cachingProvider.SetAsync(key, cacheEntry, tier.CacheEntryOption);
        }

        public void Remove(string key, params CachingTierType[] tierTypes)
        {
            tierTypes.MakeSureValid();

            foreach (var tierType in tierTypes)  
            {
                var cachingProvider = _factory.GetCachingServiceProvide(tierType);
                cachingProvider.Remove(key);    
            }
        }

        public Task RemoveAsync(string key, params CachingTierType[] tierTypes)
        {
            tierTypes.MakeSureValid();

            var tasks = tierTypes.Select(tierType => _factory.GetCachingServiceProvide(tierType).RemoveAsync(key));
            return Task.WhenAll(tasks);
        }

        public void RemoveAll(params CachingTierType[] tierTypes)
        {
            tierTypes.MakeSureValid();
            Parallel.ForEach(tierTypes, tierType =>  
            {
                var cachingProvider = _factory.GetCachingServiceProvide(tierType);
                cachingProvider.RemoveAll();
            });
        }

        public Task RemoveAllAsync(params CachingTierType[] tierTypes)
        {
            tierTypes.MakeSureValid();
            var tasks = tierTypes.Select(tierType => _factory.GetCachingServiceProvide(tierType).RemoveAllAsync());
            return Task.WhenAll(tasks);
        }
    }
}
