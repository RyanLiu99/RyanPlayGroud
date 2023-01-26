using System;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NUnit.Framework;
using SmallTests.Entities;
using SmallTests.Helpers;

namespace SmallTests
{
    internal class SerializationTest
    {
        [Test]
        public void TestSerializationSimple()
        {
            var entityDependency = new EntityDependency<Person, int>(1, 1, 2, 2);

            var collectionDependency = new string[] { "Col1", "Col1", "Coll2" };

            var valueDependencies = new ValueDependencies(new[] { entityDependency }, collectionDependency);

            //var serialized = JsonSerializer.Serialize(valueDependencies);  //cause exception
            var serialized = JsonConvert.SerializeObject(valueDependencies); //ok

            TestHelpers.Logger.Value.LogInformation("serialized: {serialized}", serialized);

            var deserialized = JsonConvert.DeserializeObject<ValueDependencies>(serialized);
            Assert.IsNotNull(deserialized);
            Assert.AreEqual(3, deserialized.CollectionDependencies.Count);
            Assert.AreEqual(4, deserialized.EntityDependencies[0].Ids.Count);

            var compressed = deserialized.Compress();
            Assert.AreEqual(2, compressed!.CollectionDependencies.Count); //one duplicate "Col1" removed


            var entityDepd = compressed?.EntityDependencies?[0];
            Assert.NotNull(entityDepd);
            Assert.AreEqual(typeof(Person).FullName, entityDepd.EntityTypeName);

            Assert.AreEqual(2, entityDepd.Ids.Count); //duplicate is removed

        }

        [Test]
        public void TestSerializationValueTuple()
        {
            var entityDependency = new EntityDependency<Person, ValueTuple<int, string>>(
                (1, "K1"),
                (1, "K1"),
                (2, "K1"),
                (2, "K2")
                );

            var collectionDependency = new string[] { "Col1", "Col1", "Coll2" };

            var valueDependencies = new ValueDependencies(new[] { entityDependency }, collectionDependency);

            //var serialized = JsonSerializer.Serialize(valueDependencies);  //cause exception
            var serialized = JsonConvert.SerializeObject(valueDependencies); //ok

            TestHelpers.Logger.Value.LogInformation("serialized: {serialized}", serialized);

            
            var deserialized = JsonConvert.DeserializeObject<ValueDependencies>(serialized); 

            Assert.IsNotNull(deserialized);
            Assert.AreEqual(3,deserialized.CollectionDependencies.Count);
            Assert.AreEqual(4, deserialized.EntityDependencies[0].Ids.Count);

            var compressed = deserialized.Compress();
            Assert.AreEqual(2, compressed.CollectionDependencies.Count); //one duplicate "Col1" removed


            var entityDepd = compressed?.EntityDependencies?[0];
            Assert.NotNull(entityDepd);
            Assert.AreEqual(typeof(Person).FullName, entityDepd.EntityTypeName);

            Assert.AreEqual(3, entityDepd.Ids.Count); //  2x {1, K1}, (2, K1), (2, K2). Duplicate is not removed yet, it is JObject

        }
    }
}
