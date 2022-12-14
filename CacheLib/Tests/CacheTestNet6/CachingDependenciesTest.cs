using Medrio.Caching.Dependencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Medrio.Caching.InMemoryInvalidationService;

namespace CacheTestNet6
{
    public class CachingDependenciesTest
    {
        [Test]
        public void TestCompress()
        {
            var x = new EntityDependency<CachingDependenciesTest>(new List<object>() { 1, 2, 3 });
            var cachingDependencies = new List<CachingDependencies>()
            {
                
                new(new List<EntityDependency>()
                    {
                        x,
                        x,
                        new EntityDependency<CachingDependenciesTest>(new List<object>(){1,2,4})
                    }, 
                    new List<CollectionDependency>()
                    {
                        new CollectionDependency<CachingDependenciesTest>(),
                        new("Collect1"),
                        new("Collect2")
                    }),

                new(new List<EntityDependency>()
                    {
                        new EntityDependency<CachingDependenciesTest, int>(new List<int>(){1})
                    },
                    new List<CollectionDependency>()
                    {
                        new CollectionDependency<CachingDependenciesTest>(),
                        new("Collect1"),
                        new("Collect3")
                    }),

                new(new List<EntityDependency>()
                    {
                        new("CachingDependenciesTest", new object[]{6,7}),
                        x
                    },
                    new List<CollectionDependency>()
                    {
                        new CollectionDependency<CachingDependenciesTest>(),
                        new("Collect2"),
                        new("Collect4")
                    }),
            };
            var result = cachingDependencies.Compress();
            Assert.NotNull(result);
            
            Assert.NotNull(result.EntityDependencies);
            Assert.That(result.EntityDependencies.Count, Is.EqualTo(2));
            
            Assert.AreEqual("CacheTestNet6.CachingDependenciesTest", result.EntityDependencies[0].EntityTypeName);
            Assert.AreEqual(4, result.EntityDependencies[0].Ids.Count);//1,2,3,4
            Assert.AreEqual(2, result.EntityDependencies[0].Ids[1]);

            Assert.AreEqual("CachingDependenciesTest", result.EntityDependencies[1].EntityTypeName);
            Assert.AreEqual(2, result.EntityDependencies[1].Ids.Count);//6, 7

            Assert.NotNull(result.CollectionDependencies);
            Assert.AreEqual(5, result.CollectionDependencies.Count); // Collect11,2, 3,4, and CachingDependenciesTest

        }
    }
}
