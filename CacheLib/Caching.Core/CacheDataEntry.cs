using Medrio.Caching.Dependencies;

namespace Medrio.Caching.Abstraction
{
    public class CacheDataEntry<T>
    {
        public T Data { get; }

        public CachingDependencies? Dependencies { get; }

        public CacheDataEntry(T data, CachingDependencies? dependencies)
        {
            Data = data;
            Dependencies = dependencies;
        }
    }
}
