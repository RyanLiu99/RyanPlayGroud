using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Medrio.Infrastructure.Ioc.Dependency;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.Caching;
using System.Threading;
using Medrio.Caching.Abstraction;
using Medrio.Caching.Abstraction.Caches;
using Medrio.Caching.Abstraction.Utilities;

[assembly: InternalsVisibleTo("CacheTestNetFramework")]

namespace Medrio.Caching.InMemoryCache
{
    [RegisterAs(typeof(IInMemoryCache), Lifetime = ServiceLifetime.Singleton)]
    internal class InMemoryCache : IInMemoryCache, IDisposable
    {
        private MemoryCache _cache;

        public InMemoryCache()
        {
            _cache = new MemoryCache($"Medrio In Memory Cache {DateTime.UtcNow}");
        }

        public bool TryGet<T>(string key, out T? data)
        {
            if (_cache.Contains(key))
            {
                data = (T?)_cache.Get(key);
                return true;
            }
            else
            {
                data = default(T?);
                return false;
            }
        }

        public Task<bool> TryGetAsync<T>(string key, out T? data)
        {
            var result = TryGet(key, out data);
            return result ? TaskResults.True : TaskResults.False;
        }

        public void Set<T>(string key, CacheDataEntry<T> cacheEntry, CacheEntryOption? cacheEntryOption)
        {
            CacheItemPolicy? policy = cacheEntryOption.ToCacheItemPolicy();
            _cache.Set(key, cacheEntry, policy);
        }

        public Task SetAsync<T>(string key, CacheDataEntry<T> cacheEntry, CacheEntryOption? cacheEntryOption)
        {
            Set(key, cacheEntry, cacheEntryOption);
            return Task.CompletedTask;
        }

        public void Remove(string key)
        {
            _cache.Remove(key, CacheEntryRemovedReason.Removed);
        }

        public Task RemoveAsync(string key)
        {
            Remove(key);
            return Task.CompletedTask;
        }

        public void RemoveAll()
        {
            var oldCache = Interlocked.Exchange(ref _cache, new MemoryCache($"Medrio In Memory Cache {DateTime.UtcNow}"));
            oldCache.Dispose();
        }

        public Task RemoveAllAsync()
        {
            RemoveAll();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _cache.Dispose();
        }
    }
}
