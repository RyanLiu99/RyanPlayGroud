using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Medrio.Caching.Abstraction.CachingServiceProviders;
using Medrio.Caching.Abstraction.Dependencies;
using Medrio.Infrastructure.Ioc.Dependency;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.Caching;

[assembly: InternalsVisibleTo("CacheTestNetFramework")]

namespace Medrio.Caching.InMemoryCache
{
    [RegisterAs(typeof(IInMemoryCacheProvider), Lifetime = ServiceLifetime.Singleton)]
    internal class InMemoryCacheProvider : IInMemoryCacheProvider
    {
        public InMemoryCacheProvider()
        {
                
        }

        public T? Get<T>(string key)
        {
            return default(T?);
        }

        public Task<T?> GetAsync<T>(string key)
        {
            throw new NotImplementedException();
        }

        public T? GetOrSet<T>(string key, Func<T> factory, CachingDependencies? dependencies = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetOrSetAsync<T>(string key, Func<T> factory, CachingDependencies? dependencies = null)
        {
            throw new NotImplementedException();
        }

        public void Set<T>(string key, T data, CachingDependencies? dependencies = null)
        {
            throw new NotImplementedException();
        }

        public Task SetAsync<T>(string key, T data, CachingDependencies? dependencies = null)
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
