using System.Collections.Generic;

namespace Medrio.Caching.Dependencies
{
    public class EntityDependency
    {
        public string EntityTypeName { get; private set; }
        public IList<object> Ids { get; private set; }

        public EntityDependency(string entityTypeName, IList<object> ids)
        {
            EntityTypeName = entityTypeName;
            Ids = ids;
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

        public new IList<TId>[] Ids { get; private set; }

        public EntityDependency(IList<TId>[] ids) : base(ids)
        {
            Ids = ids;
        }
    }
}
