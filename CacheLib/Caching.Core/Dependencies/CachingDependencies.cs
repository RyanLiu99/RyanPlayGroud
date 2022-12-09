using System.Collections.Generic;

namespace Medrio.Caching.Abstraction.Dependencies
{
    public class CachingDependencies
    {
        public IList<EntityDependency> EntityDependencies => new List<EntityDependency>();
        public IList<CollectionDependency> CollectionDependencies => new List<CollectionDependency>();
    }

    
    //public class CachingDependencies<TEntity, TId> 
    //{
    //    public IList<EntityDependency<TEntity>> EntityDependencies => new List<EntityDependency<TEntity>>();
    //    public IList<CollectionDependency<TEntity>> CollectionDependencies => new List<CollectionDependency<TEntity>>();
    //}
}
