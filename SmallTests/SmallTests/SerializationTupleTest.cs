using System;
using NUnit.Framework;
using SmallTests.Entities;
using SmallTests.Helpers.Serialization;

namespace SmallTests
{
    internal class SerializationTupleTest
    {
        [Test]
        public void TestSerializationTuple([Values(SerializerTypeEnum.MessagePack, SerializerTypeEnum.NewtonSoft)]SerializerTypeEnum type)
        {
            TestSerializationTupleImp(type);
        }

        private void TestSerializationTupleImp(SerializerTypeEnum type)
        {
            var entityDependency = new EntityDependency<Person, Tuple<int, string>>(
                new Tuple<int, string>(1, "K1"),
                new Tuple<int, string>(1, "K1"),
                new Tuple<int, string>(2, "K1"),
                new Tuple<int, string>(2, "K2")
            );

            var entityDependency2 = new EntityDependency<Person, ValueTuple<decimal, DateTime>>(
                (10.23m, DateTime.MaxValue),
                (decimal.MaxValue, DateTime.UtcNow)

            );
            var collectionDependency = new string[] { "Col1", "Col1", "Coll2" };

            var valueDependencies = new ValueDependencies(new EntityDependency[] { entityDependency }, collectionDependency);

            var deserialized = SerializerFactory.GetSerializer<ValueDependencies>(type).SerializeDeSerializeTuple(valueDependencies);

            Assert.IsNotNull(deserialized);
            Assert.AreEqual(valueDependencies.CollectionDependencies.Count, deserialized.CollectionDependencies.Count);
            Assert.AreEqual(valueDependencies.EntityDependencies[0].Ids.Count, deserialized.EntityDependencies[0].Ids.Count);


            var compressed = deserialized.Compress();
            Assert.AreEqual(2, compressed.CollectionDependencies.Count); //one duplicate "Col1" removed


            var entityDepd = compressed?.EntityDependencies?[0];
            Assert.NotNull(entityDepd);
            Assert.AreEqual(typeof(Person).FullName, entityDepd.EntityTypeName);

            Assert.AreEqual(3, entityDepd.Ids.Count); //  2x {1, K1}, (2, K1), (2, K2). For NewtonJson, Duplicate is not removed yet, it is JObject.
        }
    }
}
