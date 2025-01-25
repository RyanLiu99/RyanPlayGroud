using System;
using NUnit.Framework;
using SmallTests.Entities;
using SmallTests.Helpers.Serialization;

namespace SmallTests
{
    internal class SerializationSimpleTest
    {
     
        [Test]

        public void TestGuid([Values(SerializerTypeEnum.MessagePack, SerializerTypeEnum.NewtonSoft)] SerializerTypeEnum type)
        {
            var input = new Guid();
            var result = SerializerFactory.GetSerializer<Guid>(type).SerializeDeSerialize(input);

            Assert.AreEqual(input, result);

            var input2 = Guid.Empty;
            var result2 = SerializerFactory.GetSerializer<Guid>(type).SerializeDeSerialize(input2);

            Assert.AreEqual(input2, result2);
        }

        [Test]
        public void TestDecimal([Values(SerializerTypeEnum.MessagePack, SerializerTypeEnum.NewtonSoft)] SerializerTypeEnum type)
        {
            decimal input = 10.23m;
            var result = SerializerFactory.GetSerializer<decimal>(type).SerializeDeSerialize(input);
            Assert.AreEqual(input, result);
        }


        private void TestDateTimeImp([Values(SerializerTypeEnum.MessagePack, SerializerTypeEnum.NewtonSoft)] SerializerTypeEnum type)
        {
            var input = DateTime.UtcNow; //DateTime.Now will fail
            var result = SerializerFactory.GetSerializer<DateTime>(type).SerializeDeSerialize(input);
            Assert.AreEqual(input, result);

            var input2 = DateTime.MaxValue;
            var results = SerializerFactory.GetSerializer<DateTime>(type).SerializeDeSerialize(input2);
            Assert.AreEqual(input2, results);
        }
        
        [Test]
        public void TestSimpleUsingNewton()
        {
            TestSerializationSimpleValueDependencies(SerializationHelper.WireUsingNewton);
        }

        //[Test]
        [Ignore("Property need has both getter/setter for data contract to work")]
        public void TestSimpleUsingDataContract()
        {
            TestSerializationSimpleValueDependencies(SerializationHelper.WireUsingDataContract);
        }

        [Test]
        public void TestSimpleUsingMessagePack()
        {
            TestSerializationSimpleValueDependencies(SerializationValueDependenciesHelper.WireUsingMessagePack);
        }

        private void TestSerializationSimpleValueDependencies(Func<ValueDependencies, ValueDependencies> transform)
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
    }
}
