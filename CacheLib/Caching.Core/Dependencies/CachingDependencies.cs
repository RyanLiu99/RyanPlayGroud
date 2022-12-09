using System.Collections.Generic;

namespace Medrio.Caching.Abstraction.Dependencies
{
    public class CachingDependencies
    {
        public IList<EntityDependency> EntityDependencies => new List<EntityDependency>();
        public IList<CollectionDependency> CollectionDependencies => new List<CollectionDependency>();
    }

}
