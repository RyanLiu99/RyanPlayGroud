using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Medrio.Caching.Abstraction
{
    public static class CachingTierExtensions
    {
        public static void MakeSureValid(this IEnumerable<CachingTier> tiers)
        {
            if (tiers == null || !tiers.Any())
                throw new ArgumentException($"{nameof(tiers)} must provided.");
        }
    }
}
