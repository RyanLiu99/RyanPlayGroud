using System.Collections.Generic;
using System.Linq;

namespace Medrio.Caching.Abstraction
{
    public static class CachingTierTypeExtensions
    {
        public static IOrderedEnumerable<CachingTierType> Sort(this IEnumerable<CachingTierType> tierTypes)
        {
            return tierTypes.OrderBy(x => x);
        }

        public static CachingTierType[] GetDefaultIfNotSpecified(this CachingTierType[] tierTypes)
        {
            if (!tierTypes.Any())
            {
                return new CachingTierType[] { CachingTierType.LocalInMemory };
            }

            return tierTypes;
        }
    }
}
