using Medrio.Caching.Abstraction.Dependencies;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Medrio.Caching.Abstraction
{
    public static class CachingServiceExtension
    {

        public static T? Get<T>(this ICachingService cache, string key, params CachingTierType[] tierTypes)
        {
            var found = cache.TryGet(key, out T? result, tierTypes);
            return result;
        }



        //public static TItem? GetOrCreate<TItem>(this IMemoryCache cache, object key, Func<ICacheEntry, TItem> factory)
        //{
        //    if (!cache.TryGetValue(key, out object? result))
        //    {
        //        using ICacheEntry entry = cache.CreateEntry(key);

        //        result = factory(entry);
        //        entry.Value = result;
        //    }

        //    return (TItem?)result;
        //}

        //public static async Task<TItem?> GetOrCreateAsync<TItem>(this IMemoryCache cache, object key, Func<ICacheEntry, Task<TItem>> factory)
        //{
        //    if (!cache.TryGetValue(key, out object? result))
        //    {
        //        using ICacheEntry entry = cache.CreateEntry(key);

        //        result = await factory(entry).ConfigureAwait(false);
        //        entry.Value = result;
        //    }

        //    return (TItem?)result;
        //}


        //T GetOrCreate<T>(string key, Func<T> factory, CachingStrategy cachingStrategy = null,
        //    CachingDependencies dependencies = null);

        //Task<T> GetOrCreateAsync<T>(string key, Func<T> factory, CachingStrategy cachingStrategy = null,
        //    CachingDependencies dependencies = null);



    }
}
