using System;
using System.Threading.Tasks;
using Medrio.Caching.Abstraction;
using Medrio.Caching.Abstraction.CachingServiceProviders;
using Medrio.Infrastructure.Ioc.Dependency;
using Microsoft.Extensions.DependencyInjection;

namespace Medrio.Caching.RedisDistributedCache
{
    [RegisterAs(typeof(IDistributedCacheProvider), Lifetime = ServiceLifetime.Singleton)]
    internal class RedisDistributedCacheProvider : IDistributedCacheProvider
    {
        public bool TryGet<T>(string key, out T? data)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TryGetAsync<T>(string key, out T? data)
        {
            throw new NotImplementedException();
        }

        public void Set<T>(string key, CacheDataEntry<T> cacheEntry, CacheEntryOption? cacheEntryOption)
        {
            throw new NotImplementedException();
        }

        public Task SetAsync<T>(string key, CacheDataEntry<T> cacheEntry, CacheEntryOption? cacheEntryOption)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(string key)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll()
        {
            throw new NotImplementedException();
        }

        public Task RemoveAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
