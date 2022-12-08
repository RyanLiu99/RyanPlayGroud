using System;
using System.Threading.Tasks;
using Medrio.Caching.Abstraction.Dependencies;

namespace Medrio.Caching.Abstraction
{
    public interface ICachingService
    {
        bool TryGet<T>(string key, out T? data, params CachingTierType[] tierTypes);

        Task<T?> GetAsync<T>(string key, params CachingTierType[] tierTypes);

        void Set<T>(string key, T data, CachingTier cachingTier, CachingDependencies dependencies = null);
        Task SetAsync<T>(string key, T data, CachingTier cachingTier, CachingDependencies dependencies = null);

        void Remove(string key, params CachingTierType[] tierType);

        Task RemoveAsync(string key, params CachingTierType[] tierType);

        void RemoveAll();

         Task RemoveAllAsync();
    }
}
