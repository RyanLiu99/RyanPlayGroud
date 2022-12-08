using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Medrio.Caching.Abstraction
{
    public static class CachingTierTypeExtensions
    {
        public static IOrderedEnumerable<CachingTierType> Sort(this IEnumerable<CachingTierType> tierTypes)
        {
            return tierTypes.OrderBy(x => x);
        }
    }
}
