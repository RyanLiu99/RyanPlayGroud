using System;
using System.Threading.Tasks;
using Medrio.Caching.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TestsShared;
using TestsShared.Entity;

namespace CacheTestNetFramework
{
    public class MedrioInMemoryCacheTest
    {
        [Test]
        public void GetNullIfNotInCache()
        {
            ICachingOrchestrator cache = Setup.Container.GetRequiredService<ICachingOrchestrator>();
            var result = cache.Get<int?>(Guid.NewGuid().ToString(), CachingTierType.LocalInMemory);
            Assert.IsNull(result);
        }

        [Test]
        public void GetDefaultValueIfNotInCache()
        {
            ICachingOrchestrator cache = Setup.Container.GetRequiredService<ICachingOrchestrator>();
            var result = cache.Get<int>("TestCacheService", CachingTierType.LocalInMemory);
            Assert.AreEqual(0, result);
        }

        [Test]
        public async Task CanGetCacheBackAndExpire()
        {
            int cacheDuration = 100;
            ICachingOrchestrator cache = Setup.Container.GetRequiredService<ICachingOrchestrator>();

            string key = Guid.NewGuid().ToString();
            Person person = new Person(key);
            await cache.SetInMemoryCacheAsync(key, person, DateTimeOffset.UtcNow + TimeSpan.FromMilliseconds(cacheDuration))
                .ConfigureAwait(false);
            var personB = cache.Get<Person>(key);
            Assert.NotNull(personB);
            Assert.AreEqual(person.Name, personB.Name);

            await Task.Delay(cacheDuration).ConfigureAwait(false);
            var personC = cache.Get<Person>(key);
            Assert.IsNull(personC);
        }
    }
}