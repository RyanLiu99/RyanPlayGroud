using System.Threading.Tasks;
using Medrio.Caching.Dependencies;

namespace Medrio.Caching.Abstraction
{
    public interface ICachingOrchestrator
    {
        bool TryGet<T>(string key, out T? data, params CachingTierType[] tierTypes);

        Task<T?> GetAsync<T>(string key, params CachingTierType[] tierTypes);

        void Set<T>(string key, T data, CachingTier tier, CachingDependencies? dependencies = null);
        Task SetAsync<T>(string key, T data, CachingTier tier, CachingDependencies? dependencies = null);

        void Remove(string key, params CachingTierType[] tierTypes);

        Task RemoveAsync(string key, params CachingTierType[] tierTypes);

        void RemoveAll(params CachingTierType[] tierTypes);

         Task RemoveAllAsync(params CachingTierType[] tierTypes);
    }
}
