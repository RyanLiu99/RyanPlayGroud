namespace Medrio.Caching.Dependencies
{
    public class CollectionDependency
    {
        public string CollectionItemTypeName { get; private set; }

        public CollectionDependency(string collectionItemTypeName)
        {
            CollectionItemTypeName = collectionItemTypeName;
        }
    }

    public class CollectionDependency<T> : CollectionDependency
    {
        public CollectionDependency() : base(typeof(T).FullName)
        {
        }
    }
}