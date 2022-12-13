using Medrio.Caching.Dependencies;

namespace Medrio.Caching.Abstraction
{
    public class CacheDataEntry<T>
    {
        public T Data { get; private set; }

        public CachingDependencies? Dependencies { get; private set; }

        public CacheDataEntry(T data, CachingDependencies? dependencies)
        {
            Data = data;
            Dependencies = dependencies;
        }
    }
}
