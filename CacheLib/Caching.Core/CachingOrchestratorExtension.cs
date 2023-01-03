using System;
using System.Linq;
using System.Threading.Tasks;
using Medrio.Caching.Dependencies;

namespace Medrio.Caching.Abstraction
{
    public static class CachingOrchestratorExtension
    {

        public static T? Get<T>(this ICachingOrchestrator cache, string key, params CachingTierType[] tierTypes)
        {
            var found = cache.TryGet(key, out T? result, tierTypes);
            return result;
        }


        public static T? GetOrCreate<T>(this ICachingOrchestrator cache, string key, Func<T> factory,
            CachingDependencies? dependencies = null, params CachingTier[] tiers)
        {
            tiers.MakeSureValid();

            if (!cache.TryGet(key, out T? result, tiers.Select(t => t.TierType).Sort().ToArray()))
            {
                result = factory();
                if (result != null)
                {
                    cache.Set(key, result, tiers[0], dependencies);
                }
            }

            return result;
        }

        public static async Task<T?> GetOrCreateAsync<T>(this ICachingOrchestrator cache, string key,
            Func<Task<T>> factory,
            CachingDependencies? dependencies = null, params CachingTier[] tiers)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }


            tiers.MakeSureValid();

            var data = await cache.GetAsync<T>(key, tiers.Select(t => t.TierType).Sort().ToArray())
                .ConfigureAwait(false);

            if (data == null)
            {
                data = await factory().ConfigureAwait(false);
                if (data != null)
                {
                    await cache.SetAsync(key, data, tiers[0], dependencies).ConfigureAwait(false);
                }
            }

            return data;
        }

        public static void SetCache<T>(this ICachingOrchestrator cache, string key, T data, DateTimeOffset? absoluteExpiration=null,
            CachingDependencies? dependencies = null, CachingTierType cachingTierType = CachingTierType.LocalInMemory)
        {
            cache.Set(key, data, new CachingTier(cachingTierType, absoluteExpiration), dependencies);
        }

        public static Task SetCacheAsync<T>(this ICachingOrchestrator cache, string key, T data, DateTimeOffset? absoluteExpiration = null,
            CachingDependencies? dependencies = null, CachingTierType cachingTierType = CachingTierType.LocalInMemory)
        {
            return cache.SetAsync(key, data, new CachingTier(cachingTierType, absoluteExpiration),
                dependencies);
        }

        public static void SetCache<T>(this ICachingOrchestrator cache, string key, T data, TimeSpan? slidingExpiration = null,
            CachingDependencies? dependencies = null, CachingTierType cachingTierType = CachingTierType.LocalInMemory)
        {
            cache.Set(key, data, new CachingTier(cachingTierType, slidingExpiration), dependencies);
        }

        public static Task SetCacheAsync<T>(this ICachingOrchestrator cache, string key, T data, TimeSpan? slidingExpiration = null,
            CachingDependencies? dependencies = null, CachingTierType cachingTierType = CachingTierType.LocalInMemory)
        {
            return cache.SetAsync(key, data, new CachingTier(cachingTierType, slidingExpiration),
                dependencies);
        }
    }
}
