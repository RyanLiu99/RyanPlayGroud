using System;
using System.IO;
using System.Runtime.Serialization;
using MessagePack;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NUnit.Framework;
using SmallTests.Entities;
using SmallTests.Helpers;

namespace SmallTests
{
    internal class SerializationTest
    {
        #region test simpe
        [Test]
        public void TestSimpleUsingNewton()
        {
            TestSerializationSimple(WireUsingNewton);
        }

        [Test]
        [Ignore("Property need has both getter/setter for data contract to work")]
        public void TestSimpleUsingDataContract()
        {
            TestSerializationSimple(WireUsingDataContract);
        }

        [Test]
        public void TestSimpleUsingMessagePack()
        {
            TestSerializationSimple(WireUsingMessagePack);
        }

  
        private void TestSerializationSimple(Func<ValueDependencies, ValueDependencies> transform)
        {
            var entityDependency = new EntityDependency<Person, int>(1, 1, 2, 2);

            var collectionDependency = new string[] { "Col1", "Col1", "Coll2" };

            var valueDependencies = new ValueDependencies(new[] { entityDependency }, collectionDependency);

            var deserialized = transform(valueDependencies);
            Assert.IsNotNull(deserialized);
            Assert.AreEqual(3, deserialized.CollectionDependencies.Count);
            Assert.AreEqual(4, deserialized.EntityDependencies[0].Ids.Count);

            var compressed = deserialized.Compress();
            Assert.AreEqual(2, compressed!.CollectionDependencies.Count); //one duplicate "Col1" removed


            var entityDepd = compressed?.EntityDependencies?[0];
            Assert.NotNull(entityDepd);
            Assert.AreEqual(typeof(Person).FullName, entityDepd.EntityTypeName);

            Assert.AreEqual(2, entityDepd.Ids.Count); //[1,2],  duplicate is removed
        }

        #endregion TestSimple

        #region TestValueTuple
        [Test]
        public void TestSerializationValueTupleUsingNewton()
        {
            TestSerializationValueTuple(WireUsingNewton);
        }

        [Ignore("Not working, cannot handle open generic , nor VauleTuple")]
        [Test]
        public void TestSerializationValueTupleDataContract()
        {
            TestSerializationValueTuple(WireUsingDataContract);
        }

        [Test]
        public void TestSerializationValueTupleMessagepack()
        {
            TestSerializationValueTuple(WireUsingMessagePack);
        }

        private void TestSerializationValueTuple(Func<ValueDependencies, ValueDependencies> transform)
        {
            var entityDependency = new EntityDependency<Person, ValueTuple<int, string>>(
                (1, "K1"),
                (1, "K1"),
                (2, "K1"),
                (2, "K2")
                );

            var collectionDependency = new string[] { "Col1", "Col1", "Coll2" };

            var valueDependencies = new ValueDependencies(new[] { entityDependency }, collectionDependency);

            var deserialized = transform(valueDependencies);

            Assert.IsNotNull(deserialized);
            Assert.AreEqual(valueDependencies.CollectionDependencies.Count, deserialized.CollectionDependencies.Count);
            Assert.AreEqual(valueDependencies.EntityDependencies[0].Ids.Count, deserialized.EntityDependencies[0].Ids.Count);


            var compressed = deserialized.Compress();
            Assert.AreEqual(2, compressed.CollectionDependencies.Count); //one duplicate "Col1" removed


            var entityDepd = compressed?.EntityDependencies?[0];
            Assert.NotNull(entityDepd);
            Assert.AreEqual(typeof(Person).FullName, entityDepd.EntityTypeName);

            Assert.AreEqual(4, entityDepd.Ids.Count); //  2x {1, K1}, (2, K1), (2, K2). Duplicate is not removed yet, it is JObject

        }

        #endregion TestValueTuple

        #region transform funcs

        private static ValueDependencies WireUsingNewton(ValueDependencies input)
        {
            //var serialized = JsonSerializer.Serialize(valueDependencies);  //cause exception
            var serialized = JsonConvert.SerializeObject(input); //ok

            TestHelpers.Logger.Value.LogInformation("serialized: {serialized}", serialized);

            var deserialized = JsonConvert.DeserializeObject<ValueDependencies>(serialized);

            return deserialized;
        }


        private static ValueDependencies WireUsingDataContract(ValueDependencies input)
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(ValueDependencies));

            var stream = new MemoryStream();
            ser.WriteObject(stream, input); //ok

            stream.Seek(0, SeekOrigin.Begin);

            var deserialized = ser.ReadObject(stream) as ValueDependencies;

            return deserialized;
        }

        private static ValueDependencies WireUsingMessagePack(ValueDependencies input)
        {
            byte[] bytes = MessagePackSerializer.Serialize(input);
            ValueDependencies deserialized = MessagePackSerializer.Deserialize<ValueDependencies>(bytes);
            return deserialized;
        }
        #endregion
    }
}
