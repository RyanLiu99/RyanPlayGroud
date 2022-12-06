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

        public T Get<T>(string key, CachingStrategy cachingStrategy)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync<T>(string key, CachingStrategy cachingStrategy)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key, params CachingTierType[] tierTypes)
        {
            if (tierTypes == null || tierTypes.Length == 0)
                throw new ArgumentNullException($"{nameof(tierTypes)} must provided.");

            foreach (var tierType in tierTypes.OrderBy(x => x))  //try local cache first
            {
                var cachingProvider = _factory.GetCachingServiceProvide(tierType);
                T? result = cachingProvider.Get<T>(key);
                if (result != null)
                {
                    //TO DO: fill other tiers as needed. 
                    return result;
                }
            }
            return default(T)!;
        }

        public async Task<T> GetAsync<T>(string key, params CachingTierType[] tierTypes)
        {
            if (tierTypes == null || tierTypes.Length == 0)
                throw new ArgumentNullException($"{nameof(tierTypes)} must provided.");

            foreach (var tierType in tierTypes.OrderBy(x => x))  //try local cache first
            {
                var cachingProvider = _factory.GetCachingServiceProvide(tierType);
                T? result = await cachingProvider.GetAsync<T>(key).ConfigureAwait(false);
                if (result != null)
                {
                    //TO DO: fill other tiers as needed. 
                    return result;
                }
            }
            return default(T)!;
        }

        public T GetOrSet<T>(string key, Func<T> factory, CachingStrategy cachingStrategy = null,
            CachingDependencies dependencies = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetOrSetAsync<T>(string key, Func<T> factory, CachingStrategy cachingStrategy = null,
            CachingDependencies dependencies = null)
        {
            throw new NotImplementedException();
        }

        public void Set<T>(string key, T data, CachingStrategy cachingStrategy = null, CachingDependencies dependencies = null)
        {
            throw new NotImplementedException();
        }

        public Task SetAsync<T>(string key, T data, CachingStrategy cachingStrategy = null, CachingDependencies dependencies = null)
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
