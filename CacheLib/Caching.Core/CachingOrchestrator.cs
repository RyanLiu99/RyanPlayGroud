using System;
using System.Linq;
using System.Threading.Tasks;
using Medrio.Caching.Abstraction.Caches;
using Medrio.Caching.Dependencies;
using Medrio.Infrastructure.Ioc.Dependency;
using Microsoft.Extensions.DependencyInjection;

namespace Medrio.Caching.Abstraction
{
    [RegisterAs(typeof(ICachingOrchestrator), Lifetime = ServiceLifetime.Transient )]
    internal class CachingOrchestrator : ICachingOrchestrator
    {
        private readonly ICacheFactory _factory;

        public CachingOrchestrator(ICacheFactory factory)
        {
            _factory = factory;
        }

        public bool TryGet<T>(string key, out T? data, params CachingTierType[] tierTypes)
        {
            tierTypes.MakeSureValid();

            foreach (var tierType in tierTypes.Sort())  //try local cache first
            {
                var caches = _factory.GetCaches(tierType);
                var found = caches.TryGet(key, out data);
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
                var caches = _factory.GetCaches(tierType);
                var found = await caches.TryGetAsync<T>(key, out T? data).ConfigureAwait(false);
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
            var caches = _factory.GetCaches(tier.TierType);
            var cacheEntry = new CacheDataEntry<T>(data, dependencies);
            caches.Set(key, cacheEntry, tier.CacheEntryOption);
        }

        public Task SetAsync<T>(string key, T data, CachingTier tier, CachingDependencies? dependencies = null)
        {
            _ = tier ?? throw new ArgumentNullException(nameof(tier));
            var caches = _factory.GetCaches(tier.TierType);

            var cacheEntry = new CacheDataEntry<T>(data, dependencies);
            return caches.SetAsync(key, cacheEntry, tier.CacheEntryOption);
        }

        public void Remove(string key, params CachingTierType[] tierTypes)
        {
            tierTypes.MakeSureValid();

            foreach (var tierType in tierTypes)  
            {
                var caches = _factory.GetCaches(tierType);
                caches.Remove(key);    
            }
        }

        public Task RemoveAsync(string key, params CachingTierType[] tierTypes)
        {
            tierTypes.MakeSureValid();

            var tasks = tierTypes.Select(tierType => _factory.GetCaches(tierType).RemoveAsync(key));
            return Task.WhenAll(tasks);
        }

        public void RemoveAll(params CachingTierType[] tierTypes)
        {
            tierTypes.MakeSureValid();
            Parallel.ForEach(tierTypes, tierType =>  
            {
                var caches = _factory.GetCaches(tierType);
                caches.RemoveAll();
            });
        }

        public Task RemoveAllAsync(params CachingTierType[] tierTypes)
        {
            tierTypes.MakeSureValid();
            var tasks = tierTypes.Select(tierType => _factory.GetCaches(tierType).RemoveAllAsync());
            return Task.WhenAll(tasks);
        }
    }
}
