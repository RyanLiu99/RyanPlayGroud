using System.Threading.Tasks;
using Medrio.Caching.Dependencies;

namespace Medrio.Caching.InMemoryInvalidationService
{
    public interface IMemoryCacheInvalidator
    {
        Task InvalidateCache(CachingDependencies dependencies);
    }
}
