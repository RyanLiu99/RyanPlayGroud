using SmallTests.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SmallTests
{
    internal class DynamicTupleTest
    {
        [Test]
        public void TestEq()
        {
            var stringComposite = new Entities.DynamicTuple(new string[] { "a", "b" });
            var stringComposite2 = new Entities.DynamicTuple(new List<string> { "a", "b" });
            Assert.AreEqual(stringComposite, stringComposite2);

            var intStringComposite = new Entities.DynamicTuple(new object[] { 12, "b" });
            var intString2 = new Entities.DynamicTuple(new List<object> { 12, "b" });
            Assert.IsTrue(intStringComposite.Equals( intString2));
        }

        [Test]
        public void TestNotEq()
        {
            var stringComposite = new Entities.DynamicTuple(new string[] { "a", "b" });
            var stringComposite2 = new Entities.DynamicTuple(new List<string> { "a1", "b" });
            Assert.AreNotEqual(stringComposite, stringComposite2);
        }
    }
}
