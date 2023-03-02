using System;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using SmallTests.Entities;
using SmallTests.Helpers;
using SmallTests.Helpers.Serialization;

namespace SmallTests
{
    internal class SerializationValueTupleTest
    {


        [Test]
        public void TestSerializationValueTuple([Values(SerializerTypeEnum.MessagePack, SerializerTypeEnum.NewtonSoft)] SerializerTypeEnum type)
        {

            var entityDependency = new EntityDependency<Person, ValueTuple<int, string>>(
                (1, "K1"),
                (1, "K1"),
                (2, "K1")
            );
            var entityDependency2 = new EntityDependency<Person, ValueTuple<decimal, DateTime>>(
                (10.23m, DateTime.MaxValue),
                (10.23m, DateTime.MaxValue),
                (decimal.MaxValue, DateTime.UtcNow)

            );
            var entityDependency3 = new EntityDependency<Person, ValueTuple<double, Guid>>(
                (90.99d, Guid.Empty),
                (88.88d, Guid.NewGuid())

            );

            var collectionDependency = new string[] { "Col1", "Col1", "Coll2" };

            var valueDependencies = new ValueDependencies(new EntityDependency[] { entityDependency, entityDependency2, entityDependency3 }, collectionDependency);

            var deserialized = SerializerFactory.GetSerializer<ValueDependencies>(type).SerializeDeSerializeTuple(valueDependencies);

            Assert.IsNotNull(deserialized);
            Assert.AreEqual(valueDependencies.CollectionDependencies.Count, deserialized.CollectionDependencies.Count);
            Assert.AreEqual(valueDependencies.EntityDependencies[0].Ids.Count, deserialized.EntityDependencies[0].Ids.Count);


            var compressed = deserialized.Compress();
            Assert.AreEqual(2, compressed.CollectionDependencies.Count); //one duplicate "Col1" removed


            var entityDepd = compressed?.EntityDependencies?[0];
            Assert.NotNull(entityDepd);
            Assert.AreEqual(typeof(Person).FullName, entityDepd.EntityTypeName);

            TestHelpers.Logger.Value.LogInformation(">>>>>>>>>>>>>>>>>>");
            var i = 0;
            entityDepd.Ids.ForEach(id =>
            {
                TestHelpers.Logger.Value.LogInformation(
                    $"{i++} {((DynamicTuple)id).ToDebugString()}"
                    ); 

            });
            TestHelpers.Logger.Value.LogInformation("<<<<<<<<<<<<<<<<<");
            Assert.AreEqual(6, entityDepd.Ids.Count, "Duplicates was not removed. Ids are "); //  2x {1, K1}, (2, K1), (2, K2). Duplicate is not removed yet, it is JObject
        }
    }
}
