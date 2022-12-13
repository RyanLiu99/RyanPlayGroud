using System;
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

        public static void MakeSureValid(this IEnumerable<CachingTierType> tierTypes)
        {
            if (tierTypes == null || !tierTypes.Any())
                throw new ArgumentException($"{nameof(tierTypes)} must provided.");
        }
    }
}
