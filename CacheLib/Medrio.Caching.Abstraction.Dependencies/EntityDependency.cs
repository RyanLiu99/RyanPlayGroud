using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Medrio.Caching.Dependencies
{
    public class EntityDependency 
    {
        public string EntityTypeName { get; }
        public IList<object> Ids { get; }

        public EntityDependency(string entityTypeName, in IList<object> ids)
        {
            EntityTypeName = string.IsNullOrWhiteSpace(entityTypeName) 
                ? throw new ArgumentException(nameof(entityTypeName)) : entityTypeName;
            Ids = ids ?? throw new ArgumentNullException(nameof(ids));
        }

        public int GetHashCode(object obj)
        {
            return base.GetHashCode();
        }
    }

    public class EntityDependency<TEntity> : EntityDependency
    {
        public EntityDependency(IList<object> ids) : base(typeof(TEntity).FullName, ids)
        {
        }
    }

    public class EntityDependency<TEntity, TId> : EntityDependency<TEntity>
    {

        public new IList<TId> Ids { get; }

        public EntityDependency(IList<TId> ids) : base(ids.Cast<object>().ToList())
        {
            Ids = ids;
        }
    }
}
