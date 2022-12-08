using System;
using System.Threading.Tasks;
using Medrio.Caching.Abstraction.Dependencies;

namespace Medrio.Caching.Abstraction.CachingServiceProviders
{
    public interface ICachingServiceProvide
    {
        bool TryGet<T>(string key, out T? data);

        Task<bool> TryGetAsync<T>(string key, out T? data);

        void Set<T>(string key, T data, CacheEntryOption? cacheEntryOption, CachingDependencies? dependencies = null);
        Task SetAsync<T>(string key, T data, CacheEntryOption? cacheEntryOption, CachingDependencies? dependencies = null);

        void Remove(string key);

        Task RemoveAsync(string key);

        void RemoveAll();

        Task RemoveAllAsync();
    }
}
