using System;
using System.Threading.Tasks;

namespace Medrio.Caching.Abstraction.CachingServiceProviders
{
    public interface ICachingProvide
    {
        bool TryGet<T>(string key, out T? data);

        Task<bool> TryGetAsync<T>(string key, out T? data);

        void Set<T>(string key, CacheDataEntry<T> cacheEntry, CacheEntryOption? cacheEntryOption);

        Task SetAsync<T>(string key, CacheDataEntry<T> cacheEntry, CacheEntryOption? cacheEntryOption);

        void Remove(string key);

        Task RemoveAsync(string key);

        void RemoveAll();

        Task RemoveAllAsync();
    }
}
