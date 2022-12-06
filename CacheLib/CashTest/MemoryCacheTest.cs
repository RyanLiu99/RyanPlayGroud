using CacheTestNetFramework.Entity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CacheTestNetFramework
{
    public class MemoryCacheTest
    {
        [Test]
        public void Test1()
        {
            var cache = Setup.Container.GetRequiredService<IMemoryCache>();

            string key = "key1";
            Person person1 = new Person("P1");
            cache.Set(key, person1);
            var person1B = cache.Get<Person>(key);
            Assert.NotNull(person1B);
            Assert.AreEqual(person1.Name, person1B.Name);


            cache.Set("int", 0);
            var intVa = cache.Get<int>("int");


            cache.Dispose();
        }
    }
}