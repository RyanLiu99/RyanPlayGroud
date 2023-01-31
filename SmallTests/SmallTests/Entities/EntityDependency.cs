using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SmallTests.Entities
{
    [MessagePackObject]
    [DataContract]
    [KnownType(typeof(EntityDependency<>))]
    [KnownType(typeof(EntityDependency<,>))]
    [KnownType(typeof(EntityDependency<Person, int>))]
    [KnownType(typeof(EntityDependency<Person, ValueTuple<int, string>>))]
    
    public class EntityDependency 
    {
        //setter needed for both Newtonsoft and MessagePack
        [DataMember]
        [Key(0)]
        public string EntityTypeName { get; }

        [DataMember]
        [Key(1)]
        public IList<object> Ids { get; private set; }

        //[MessagePack.SerializationConstructor], not needed
        public EntityDependency(string entityTypeName, IList<object> ids)
        {
            EntityTypeName = string.IsNullOrWhiteSpace(entityTypeName) 
                ? throw new ArgumentException(nameof(entityTypeName)) : entityTypeName;
            Ids = ids ?? throw new ArgumentNullException(nameof(ids));
        }

        public void ReSetIds(List<DynamicTuple> newIds)
        {
            if (newIds == null)
            {
                throw new ArgumentNullException(nameof(newIds));
            }

            Ids = newIds.Cast<object>().ToList();
        }

    }


    //TId should be value type or string
    [DataContract]

    public class EntityDependency<TId> : EntityDependency 
    {
        [DataMember]
        public new IList<TId> Ids { get; }

        [SerializationConstructor]
        public EntityDependency(string entityTypeName, IList<TId> ids) : base(entityTypeName, ids.Cast<object>().ToList())
        {
            Ids = ids;
        }

        public EntityDependency(string entityTypeName, params TId[] ids) : base(entityTypeName, ids.Cast<object>().ToList())
        {
            Ids = ids;
        }
    }

    [DataContract]

    public class EntityDependency<TEntity, TId> : EntityDependency<TId> 
    {
        [SerializationConstructor]
        public EntityDependency(IList<TId> ids) : base(typeof(TEntity).FullName, ids)
        {
        }
        public EntityDependency(params TId[] ids) : base(typeof(TEntity).FullName, ids)
        {
        }
    }
}
