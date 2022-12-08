using Medrio.Caching.Abstraction;
using Medrio.Caching.Abstraction.CachingServiceProviders;
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
            ICachingServiceProvide provider = new InMemoryCacheProvider();
            provider.TryGet<int?>("any", out int? result);
            Assert.IsNull(result);
        }


        [Test]
        public void TestCacheServiceNullable()
        {
            ICachingService service = Setup.Container.GetRequiredService<ICachingService>();
            var result = service.Get<int?>("TestCacheService", CachingTierType.LocalInMemory);
            Assert.IsNull(result);
        }

        [Test]
        public void TestCacheServiceZero()
        {
            ICachingService service = Setup.Container.GetRequiredService<ICachingService>();
            var result = service.Get<int>("TestCacheService", CachingTierType.LocalInMemory);
            Assert.AreEqual(0, result);
        }
    }
}