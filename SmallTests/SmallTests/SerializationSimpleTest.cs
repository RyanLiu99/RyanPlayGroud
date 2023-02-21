using System;
using NUnit.Framework;
using SmallTests.Entities;
using SmallTests.Helpers.Serialization;

namespace SmallTests
{
    internal class SerializationSimpleTest
    {
        #region test guid
        [Test]
        public void TestGuidMessagePack()
        {
            TestGuidImp(SerializationHelper.WireUsingMessagePack);
        }

        private void TestGuidImp(Func<Guid, Guid> transform)
        {
            var guid = new Guid();
            var result = transform(guid);
            Assert.AreEqual(guid, result);

            guid = Guid.Empty;
            result = transform(guid);
            Assert.AreEqual(guid, result);
        }

        #endregion test guid

        #region test decimal
        [Test]
        public void TestDecimal()
        {
            TestDecimalImp(SerializationHelper.WireUsingMessagePack);
        }

        private void TestDecimalImp(Func<decimal, decimal> transform)
        {
            decimal input = 10.23m;
            var result = transform(input);
            Assert.AreEqual(input, result);
        }

        #endregion test guid

        #region test datetime
        [Test]
        public void TestDateTime()
        {
            TestDateTimeImp(SerializationHelper.WireUsingMessagePack);
        }

        private void TestDateTimeImp(Func<DateTime, DateTime> transform)
        {
            DateTime input = DateTime.UtcNow; //DateTime.Now will fail
            var result = transform(input);
            Assert.AreEqual(input, result);
        }

        #endregion test guid

        #region test simpe
        [Test]
        public void TestSimpleUsingNewton()
        {
            TestSerializationSimple(SerializationHelper.WireUsingNewton);
        }

        //[Test]
        [Ignore("Property need has both getter/setter for data contract to work")]
        public void TestSimpleUsingDataContract()
        {
            TestSerializationSimple(SerializationHelper.WireUsingDataContract);
        }

        [Test]
        public void TestSimpleUsingMessagePack()
        {
            TestSerializationSimple(SerializationValueDependenciesHelper.WireUsingMessagePack);
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
        
    }
}
