using System;
using System.Threading.Tasks;
using Medrio.Caching.Abstraction;
using Medrio.Caching.Abstraction.Caches;
using Medrio.Caching.RedisDistributedCache.Configurations;
using Medrio.Infrastructure.Ioc.Dependency;
using Medrio.Infrastructure.RedisClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Medrio.Caching.RedisDistributedCache
{
    [RegisterAs(typeof(IDistributedCache), Lifetime = ServiceLifetime.Singleton)]
    internal class RedisDistributedCache : IDistributedCache
    {
        //TODO: pre load script to server
        // KEYS[1] = = key
        // ARGV[1] = absolute-expiration - ticks as long (-1 for none)
        // ARGV[2] = sliding-expiration - ticks as long (-1 for none)
        // ARGV[3] = relative-expiration (long, in seconds, -1 for none) - Min(absolute-expiration - Now, sliding-expiration)
        // ARGV[4] = data - byte[]
        // this order should not change LUA script depends on it
        private const string SetScript = (@"
                redis.call('HSET', KEYS[1], 'absexp', ARGV[1], 'sldexp', ARGV[2], 'data', ARGV[4], 'dependencies', ARGV[5])
                if ARGV[3] ~= '-1' then
                  redis.call('EXPIRE', KEYS[1], ARGV[3])
                end
                return 1");


        private const string AbsoluteExpirationKey = "absexp";
        private const string SlidingExpirationKey = "sldexp";
        private const string DataKey = "data";
        private const long NotPresent = -1;
        
        private IDatabase _cache;
        private readonly string _instance;

        public RedisDistributedCache(IRedisConnectionProvider redisConnectionProvider,
            IOptions<RedisDistributedCacheOptions> redisDistributedCacheOptions)
        {
            _cache = redisConnectionProvider.GetRedisConnection(redisDistributedCacheOptions.Value.MessageBrokerIdentifier).GetDatabase();
            // This allows partitioning a single backend cache for use with multiple apps/services.
            _instance = redisDistributedCacheOptions.Value.InstanceName ?? string.Empty;
        }

        public bool TryGet<T>(string key, out T? data)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
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

        private byte[]? GetAndRefresh(string key, bool getData)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            
            // This also resets the LRU status as desired.
            // TODO: Can this be done in one operation on the server side? Probably, the trick would just be the DateTimeOffset math.
            RedisValue[] results;
            if (getData)
            {
                results = _cache.HashMemberGet(_instance + key, AbsoluteExpirationKey, SlidingExpirationKey, DataKey);
            }
            else
            {
                results = _cache.HashMemberGet(_instance + key, AbsoluteExpirationKey, SlidingExpirationKey);
            }

            // TODO: Error handling
            if (results.Length >= 2)
            {
                MapMetadata(results, out DateTimeOffset? absExpr, out TimeSpan? sldExpr);
                Refresh(_cache, key, absExpr, sldExpr);
            }

            if (results.Length >= 3 && results[2].HasValue)
            {
                return results[2];
            }

            return null;
        }


        private static void MapMetadata(RedisValue[] results, out DateTimeOffset? absoluteExpiration, out TimeSpan? slidingExpiration)
        {
            absoluteExpiration = null;
            slidingExpiration = null;
            var absoluteExpirationTicks = (long?)results[0];
            if (absoluteExpirationTicks.HasValue && absoluteExpirationTicks.Value != NotPresent)
            {
                absoluteExpiration = new DateTimeOffset(absoluteExpirationTicks.Value, TimeSpan.Zero);
            }
            var slidingExpirationTicks = (long?)results[1];
            if (slidingExpirationTicks.HasValue && slidingExpirationTicks.Value != NotPresent)
            {
                slidingExpiration = new TimeSpan(slidingExpirationTicks.Value);
            }
        }

        private void Refresh(IDatabase cache, string key, DateTimeOffset? absExpr, TimeSpan? sldExpr)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            // Note Refresh has no effect if there is just an absolute expiration (or neither).
            if (sldExpr.HasValue)
            {
                TimeSpan? expr;
                if (absExpr.HasValue)
                {
                    var relExpr = absExpr.Value - DateTimeOffset.Now;
                    expr = relExpr <= sldExpr.Value ? relExpr : sldExpr;
                }
                else
                {
                    expr = sldExpr;
                }
                cache.KeyExpire(_instance + key, expr);
                // TODO: Error handling
            }
        }
    }
}
