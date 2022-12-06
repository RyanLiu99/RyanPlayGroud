using System;
using System.Threading.Tasks;
using Medrio.Caching.Abstraction.Dependencies;

namespace Medrio.Caching.Abstraction.CachingServiceProviders
{
    public interface ICachingServiceProvide
    {
        T? Get<T>(string key);
        Task<T?> GetAsync<T>(string key);

        T? GetOrSet<T>(string key, Func<T> factory, CachingDependencies? dependencies = null);
        Task<T> GetOrSetAsync<T>(string key, Func<T> factory, CachingDependencies? dependencies = null);

        void Set<T>(string key, T data, CachingDependencies? dependencies = null);
        Task SetAsync<T>(string key, T data,  CachingDependencies? dependencies = null);

        void Remove(string key);

        Task RemoveAsync(string key);

        void RemoveAll();

        Task RemoveAllAsync();
    }
}
