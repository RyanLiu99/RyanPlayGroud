using System;
using System.Linq;
using System.Threading.Tasks;
using Medrio.Caching.Abstraction.CachingServiceProviders;
using Medrio.Caching.Abstraction.Dependencies;
using Medrio.Infrastructure.Ioc.Dependency;
using Microsoft.Extensions.DependencyInjection;

namespace Medrio.Caching.Abstraction
{
    [RegisterAs(typeof(ICachingService), Lifetime = ServiceLifetime.Transient )]
    internal class CachingService : ICachingService
    {
        private readonly ICachingProviderFactory _factory;

        public CachingService(ICachingProviderFactory factory)
        {
            _factory = factory;
        }


        public bool TryGet<T>(string key, out T? data, params CachingTierType[] tierTypes)
        {
            if (tierTypes == null || tierTypes.Length == 0)
                throw new ArgumentNullException($"{nameof(tierTypes)} must provided.");

            foreach (var tierType in tierTypes.Sort())  //try local cache first
            {
                var cachingProvider = _factory.GetCachingServiceProvide(tierType);
                var found = cachingProvider.TryGet(key, out data);
                if (found)
                {
                    return found;
                }
            }

            data = default(T?);
            return false;
        }


        public async Task<T?> GetAsync<T>(string key, params CachingTierType[] tierTypes)
        {
            if (tierTypes == null || tierTypes.Length == 0)
                throw new ArgumentNullException($"{nameof(tierTypes)} must provided.");

            foreach (var tierType in tierTypes.Sort())  //try local cache first
            {
                var cachingProvider = _factory.GetCachingServiceProvide(tierType);
                var found = await cachingProvider.TryGetAsync<T>(key, out T? data).ConfigureAwait(false);
                if (found)
                {
                    return data;
                }
            }
            return  default(T?);
        }

        public void Set<T>(string key, T data, CachingTier cachingTier, CachingDependencies dependencies = null)
        {
            throw new NotImplementedException();
        }

        public Task SetAsync<T>(string key, T data, CachingTier cachingTier, CachingDependencies dependencies = null)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key, params CachingTierType[] tierType)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(string key, params CachingTierType[] tierType)
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
