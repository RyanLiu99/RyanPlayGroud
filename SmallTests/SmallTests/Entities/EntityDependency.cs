using System;
using System.Collections.Generic;
using System.Linq;

namespace SmallTests.Entities
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
    }

    //TId should be value type or string
    public class EntityDependency<TId> : EntityDependency 
    {
        public new IList<TId> Ids { get; }

        public EntityDependency(string entityTypeName, IList<TId> ids) : base(entityTypeName, ids.Cast<object>().ToList())
        {
            Ids = ids;
        }

        public EntityDependency(string entityTypeName, params TId[] ids) : base(entityTypeName, ids.Cast<object>().ToList())
        {
            Ids = ids;
        }
    }


    public class EntityDependency<TEntity, TId> : EntityDependency<TId> where TId: struct
    {
        public EntityDependency(IList<TId> ids) : base(typeof(TEntity).FullName, ids)
        {
        }
        public EntityDependency(params TId[] ids) : base(typeof(TEntity).FullName, ids)
        {
        }
    }
}
