using System.Collections.Generic;
using System.Linq;

namespace SmallTests.Entities
{
    public static class ValueDependenciesExtensions
    {
        public static bool IsNullOrEmpty(this ValueDependencies? cachingDependencies)
        {
            if (cachingDependencies == null) return true;

            return cachingDependencies.IsEmpty();
        }

        public static ValueDependencies? Compress(this ICollection<ValueDependencies> dependencies)
        {
            var c = dependencies.Count;

            if (c == 0)
            {
                return null;
            }
            
            var entityDependencies = dependencies
                .SelectMany(d => d.EntityDependencies)
                .Distinct()
                .GroupBy(ed => ed.EntityTypeName, (entityName, items) =>
                    new EntityDependency(entityName, items.SelectMany(item => item.Ids).Distinct().ToList())
                );

            var collectionDependencies = dependencies.SelectMany(d => d.CollectionDependencies).Distinct();

            var result = new ValueDependencies(entityDependencies, collectionDependencies);
            return result;
        }

        public static ValueDependencies? Compress(this ValueDependencies? dependencies)
        {
            if(dependencies == null) return null;
            return Compress(new[] { dependencies });
        }
    }
}
