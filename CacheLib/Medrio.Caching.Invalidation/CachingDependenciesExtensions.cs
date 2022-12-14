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

            if (c == 0)
            {
                return null;
            }

            if (c == 1)
            {
                return dependencies.ElementAt(0);
            }

            var entityDependencies = dependencies.SelectMany(d => d.EntityDependencies).Distinct()
                .GroupBy( ed => ed.EntityTypeName, (entityName, items) => 
                    new EntityDependency(entityName, items.SelectMany( item =>item.Ids).Distinct().ToList()));

            var collectionDependencies = dependencies.SelectMany(d => d.CollectionDependencies).Distinct();

            var result = new CachingDependencies(entityDependencies, collectionDependencies);
            return result;
        }
    }
}
