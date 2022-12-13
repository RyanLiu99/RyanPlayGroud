using System;

namespace Medrio.Caching.Abstraction
{
    /// <summary>
    /// Caching tier: Caching type and CacheEntryOptions(mainly Expiration).
    /// <remarks>System.Runtime.Caching does not support both absoluteExpiration and slidingExpiration, so constructor to take both.</remarks>
    /// </summary>
    public class CachingTier
    {
        public CachingTierType TierType { get; private set; }

        public CacheEntryOptions? CacheEntryOption { get; private set; }

        public CachingTier(CachingTierType tierType)
        {
            TierType = tierType;
        }

        public CachingTier(CachingTierType tierType, CacheEntryOptions? cacheEntryOption)
        {
            TierType = tierType;
            CacheEntryOption = cacheEntryOption;
        }

        public CachingTier(CachingTierType tierType, DateTimeOffset? absoluteExpiration)
        {
            TierType = tierType;
            CacheEntryOption = new CacheEntryOptions(absoluteExpiration);
        }
        public CachingTier(CachingTierType tierType, TimeSpan? slidingExpiration)
        {
            TierType = tierType;
            CacheEntryOption = new CacheEntryOptions(slidingExpiration);
        }

    }
}
