using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Text;
using Medrio.Caching.Abstraction;

namespace Medrio.Caching.InMemoryCache
{
    internal static class CacheEntryOptionExtension
    {
        public static CacheItemPolicy ToCacheItemPolicy(this CacheEntryOption? option)
        {
            if (option == null) return null!;
            var result = new CacheItemPolicy();

            if(option.AbsoluteExpiration.HasValue)
                result.AbsoluteExpiration = option.AbsoluteExpiration.Value;
            if (option.SlidingExpiration.HasValue)
                result.SlidingExpiration = option.SlidingExpiration.Value;
            return result;
        }
    }
}
