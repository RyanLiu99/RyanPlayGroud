using Medrio.Caching.Abstraction;
using Medrio.Caching.Abstraction.Caches;
using Medrio.Caching.InMemoryCache;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CacheTestNetFramework
{
    public class MedrioCacheTest
    {
        [Test]
        public void TestCache()
        {
            ICache cache = new InMemoryCache();
            cache.TryGet<int?>("any", out int? result);
            Assert.IsNull(result);
        }


        [Test]
        public void TestCacheServiceNullable()
        {
            ICachingOrchestrator service = Setup.Container.GetRequiredService<ICachingOrchestrator>();
            var result = service.Get<int?>("TestCacheService", CachingTierType.LocalInMemory);
            Assert.IsNull(result);
        }

        [Test]
        public void TestCacheServiceZero()
        {
            ICachingOrchestrator service = Setup.Container.GetRequiredService<ICachingOrchestrator>();
            var result = service.Get<int>("TestCacheService", CachingTierType.LocalInMemory);
            Assert.AreEqual(0, result);
        }
    }
}