using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Medrio.Caching.Abstraction.CachingServiceProviders;
using Medrio.Caching.Abstraction.Dependencies;
using Medrio.Infrastructure.Ioc.Dependency;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.Caching;
using Medrio.Caching.Abstraction;
using Medrio.Caching.Abstraction.Utilities;

[assembly: InternalsVisibleTo("CacheTestNetFramework")]

namespace Medrio.Caching.InMemoryCache
{
    [RegisterAs(typeof(IInMemoryCacheProvider), Lifetime = ServiceLifetime.Singleton)]
    internal class InMemoryCacheProvider : IInMemoryCacheProvider, IDisposable
    {
        private readonly MemoryCache _cache;

        public InMemoryCacheProvider()
        {
            _cache = new MemoryCache("Medrio.NoDependencies");
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

        public void Set<T>(string key, T data, CacheEntryOption? cacheEntryOption, CachingDependencies? dependencies = null)
        {
            CacheItemPolicy? policy = cacheEntryOption.ToCacheItemPolicy();
            _cache.Set(key, data!, policy);
        }

        public Task SetAsync<T>(string key, T data, CacheEntryOption? cacheEntryOption, CachingDependencies? dependencies = null)
        {
            Set(key, data, cacheEntryOption);
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
            throw new NotImplementedException();
        }

        public Task RemoveAllAsync()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _cache.Dispose();
        }
    }
}
