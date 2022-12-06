using System;

namespace Medrio.Caching.Abstraction
{
    public class CachingTier
    {
        public CachingTierType TierType { get; private set; }

        public DateTimeOffset Expiration { get; set; }

        public CachingTier(CachingTierType tierType)
        {
            TierType = tierType;
        }
    }
}
