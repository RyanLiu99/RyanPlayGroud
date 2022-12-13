namespace Medrio.Caching.Abstraction
{
    public class CachingTier
    {
        public CachingTierType TierType { get; private set; }

        public CacheEntryOption? CacheEntryOption { get; private set; }

        public CachingTier(CachingTierType tierType)
        {
            TierType = tierType;
        }

        public CachingTier(CachingTierType tierType, CacheEntryOption cacheEntryOption)
        {
            TierType = tierType;
            CacheEntryOption = cacheEntryOption;
        }
    }
}
