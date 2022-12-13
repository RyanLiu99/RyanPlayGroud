using System;
using System.Collections.Generic;
using System.Linq;
using Medrio.Caching.Dependencies;

namespace Medrio.Caching.InMemoryInvalidationService
{
    public static class CachingDependenciesExtensions
    {
        public static CachingDependencies? Compress(this ICollection<CachingDependencies> dependencies)
        {
            var c = dependencies.Count;

            if (c == 0) return null;

            if (c == 1)
                return dependencies.ElementAt(0);

            throw new NotImplementedException();
        }
    }
}
