using System;
using System.Collections.Generic;
using System.Linq;

namespace Medrio.Caching.Dependencies
{
    public class CachingDependencies
    {
        public IList<EntityDependency> EntityDependencies { get; }
        public IList<CollectionDependency> CollectionDependencies { get; }

        public CachingDependencies()
        {
            EntityDependencies = new List<EntityDependency>();
            CollectionDependencies = new List<CollectionDependency>();
        }

        public CachingDependencies(IEnumerable<EntityDependency> entityDependencies, IEnumerable<CollectionDependency> collectionDependencies)
        {
            EntityDependencies = entityDependencies?.ToList() ?? throw new ArgumentNullException(nameof(entityDependencies));
            CollectionDependencies = collectionDependencies.ToList() ?? throw new ArgumentNullException(nameof(collectionDependencies)); 
        }
    }
}
