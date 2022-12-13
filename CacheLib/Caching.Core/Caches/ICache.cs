using System.Threading.Tasks;

namespace Medrio.Caching.Abstraction.Caches
{
    public interface ICache
    {
        bool TryGet<T>(string key, out T? data);

        Task<bool> TryGetAsync<T>(string key, out T? data);

        void Set<T>(string key, CacheDataEntry<T> cacheEntry, CacheEntryOptions? cacheEntryOption);

        Task SetAsync<T>(string key, CacheDataEntry<T> cacheEntry, CacheEntryOptions? cacheEntryOption);

        void Remove(string key);

        Task RemoveAsync(string key);

        void RemoveAll();

        Task RemoveAllAsync();
    }
}
