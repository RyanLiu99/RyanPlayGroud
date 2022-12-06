using System.Collections.Generic;

namespace Medrio.Caching.Abstraction.Dependencies
{
    public class CachingDependencies
    {
        public ICollection<EntityDependency> EntityDependencies => new List<EntityDependency>();
        public ICollection<CollectionDependency> CollectionDependencies => new List<CollectionDependency>();
    }
}
