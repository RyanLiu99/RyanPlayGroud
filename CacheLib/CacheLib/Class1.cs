using System;
using Microsoft.Extensions.Caching.Memory;

namespace CacheLib
{
    public class Class1
    {
        public Class1(IMemoryCache memoryCache)
        {
            memoryCache.Set<object>(new object(), new object());

        }
    }
}
