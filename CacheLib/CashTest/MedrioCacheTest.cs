using System;
using System.Threading.Tasks;
using CacheTestNetFramework.Entity;
using Medrio.Caching.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CacheTestNetFramework
{
    public class MedrioCacheTest
    {
        [Test]
        public void GetNullIfNotInCache()
        {
            ICachingOrchestrator cache = Setup.Container.GetRequiredService<ICachingOrchestrator>();
            var result = cache.Get<int?>(Guid.NewGuid().ToString(), CachingTierType.LocalInMemory);
            Assert.IsNull(result);
        }

        [Test]
        public async Task CanGetCacheBackAndExpire()
        {
            ICachingOrchestrator cache = Setup.Container.GetRequiredService<ICachingOrchestrator>();

            string key = Guid.NewGuid().ToString();
            Person person = new Person(key);
            await cache.SetInMemoryCacheAsync(key, person, DateTimeOffset.UtcNow + TimeSpan.FromMilliseconds(200)).ConfigureAwait(false);
            var personB = cache.Get<Person>(key);
            Assert.NotNull(personB);
            Assert.AreEqual(person.Name, personB.Name);

            await Task.Delay(200).ConfigureAwait(false);
            var personC = cache.Get<Person>(key);
            Assert.IsNull(personC);
        }

        [Test]
        public void GetDefaultValueIfNotInCache()
        {
            ICachingOrchestrator cache = Setup.Container.GetRequiredService<ICachingOrchestrator>();
            var result = cache.Get<int>("TestCacheService", CachingTierType.LocalInMemory);
            Assert.AreEqual(0, result);
        }
    }
}