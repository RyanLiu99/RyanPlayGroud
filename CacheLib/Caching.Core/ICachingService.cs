using System;
using System.Threading.Tasks;
using Medrio.Caching.Abstraction.Dependencies;

namespace Medrio.Caching.Abstraction
{
    public interface ICachingService
    {
        T Get<T>(string key, CachingStrategy cachingStrategy); //support one tier fill another if multiple tiers
        Task<T> GetAsync<T>(string key, CachingStrategy cachingStrategy);

        T Get<T>(string key, params CachingTierType[] tierTypes);    //no  fill since don’t know expiration, and most times one tier anyway. 

        Task<T> GetAsync<T>(string key, params CachingTierType[] tierTypes);

        T GetOrSet<T>(string key, Func<T> factory, CachingStrategy cachingStrategy = null,
            CachingDependencies dependencies = null);
        Task<T> GetOrSetAsync<T>(string key, Func<T> factory, CachingStrategy cachingStrategy = null,
            CachingDependencies dependencies = null);

        void Set<T>(string key, T data, CachingStrategy cachingStrategy = null, CachingDependencies dependencies = null);
        Task SetAsync<T>(string key, T data, CachingStrategy cachingStrategy = null, CachingDependencies dependencies = null);

        void Remove(string key, params CachingTierType[] tierType);

        Task RemoveAsync(string key, params CachingTierType[] tierType);

        void RemoveAll();

         Task RemoveAllAsync();
    }
}
