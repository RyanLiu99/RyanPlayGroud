using System;
using System.Collections.Generic;
using System.Linq;

namespace SmallTests.Helpers
{
    public static class CollectionExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> src, Action<T> action)
        {
            if (src == null)
                return;
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            foreach (T obj in src)
                action(obj);
        }

        public static bool IsNullOfEmpty<T>(this IEnumerable<T> src) => src == null || !src.Any<T>();
    }
}
