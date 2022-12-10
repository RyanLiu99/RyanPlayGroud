using Medrio.Caching.Abstraction;
using Medrio.Caching.Abstraction.CachingProviders;
using Medrio.Caching.InMemoryCache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CacheTestNetFramework
{
    public class MedrioCacheTest
    {
        [Test]
        public void TestCacheProvider()
        {
            ICachingProvide provider = new InMemoryCacheProvider();
            provider.TryGet<int?>("any", out int? result);
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