using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SmallTests.Entities;

namespace SmallTests
{
    internal class TupleTests
    {
        [Test]
        public void TestTupleEqualByItemsButNotByRef()
        {
            var a = Tuple.Create(1, "a");
            var b = Tuple.Create(1, "a");
            Assert.AreEqual(a, b);
            Assert.IsFalse(a == b);
        }

        [Test]
        public void TestValueTupleEqualByItemsAndByRef()
        {
            var a = ValueTuple.Create(1, "a");
            var b = (1, "a");
            Assert.AreEqual(a, b);
            Assert.IsTrue(a == b);
        }

        [Test]
        public void TestCreateValueTupleDynamically()
        {
            object[] a = new object[] { 1, 1, "1", "1", "a", "a", 'a', 'a' }; //final result Item1 will be object, object{int} or object{string}
            //dynamic[] a = new dynamic[] { 1, 1, "1", "1", "a", "a", 'a', 'a' };  //final result Item1 will be int or string, char

            var result = a.Select(x => ValueTuple.Create(x)).ToArray();
            Assert.AreEqual(8, result.Length);

            result = result.Distinct().ToArray();
            Assert.AreEqual(4, result.Length);
        }

        [Test]
        public void ValueTupleInCanBeDictionaryKey()
        {
            var p1 = new Person("1a");
            var p1x = new Person("1ax");
            var p2 = new Person("2b");

            var dict = new Dictionary<ValueTuple<int, string>, Person>()
            {
                { (1, "a"), p1 },
                { (2, "b"), p2 },
            };


            Assert.AreEqual(2, dict.Count);
            Assert.AreEqual(p1, dict[(1, "a")]);
        }
    }
}
