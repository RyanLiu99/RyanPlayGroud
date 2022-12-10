using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medrio.Caching.Dependencies;

namespace Medrio.Caching.Abstraction
{
    public static class CachingServiceExtension
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

        public static async Task<T?> GetOrCreateAsync<T>(this ICachingOrchestrator cache, string key, Func<Task<T>> factory,
            CachingDependencies? dependencies = null, params CachingTier[] tiers)
        {
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
    }
}
