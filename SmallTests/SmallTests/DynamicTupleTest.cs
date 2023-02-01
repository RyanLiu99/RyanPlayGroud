using SmallTests.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
            var stringComposite = new DynamicTuple(new string[] { "a", "b" });
            var stringComposite2 = new DynamicTuple(new List<string> { "a", "b" });
            Assert.AreEqual(stringComposite,   stringComposite2);
            //Assert.IsTrue(stringComposite == stringComposite2);// Cause compile error , ambiguous overload. Fix as follow, any of 3 works
            Assert.IsTrue( stringComposite == (ITuple)stringComposite2);// override ==
            Assert.IsTrue((ITuple)stringComposite == stringComposite2); //override ==

            Assert.IsFalse((ITuple)stringComposite == (ITuple)stringComposite2); // this is false, this is  ref equal!!!


            var intStringComposite = new DynamicTuple(new object[] { 12, "b" });
            var intString2 = new DynamicTuple(new List<object> { 12, "b" });
            Assert.IsTrue(intStringComposite.Equals( intString2));
        }

        [Test]
        public void TestNotEq()
        {
            var stringComposite = new DynamicTuple(new string[] { "a", "b" });
            var stringComposite2 = new DynamicTuple(new List<string> { "a1", "b" });
            Assert.AreNotEqual(stringComposite, stringComposite2);
        }

        [Test]
        public void TestNull()
        {
            var stringComposite = new DynamicTuple(new string[] { "a", "b" });
            var nullTuple = null as Tuple<string, string>;
            Assert.AreNotEqual(stringComposite, nullTuple);
            Assert.IsFalse(stringComposite.Equals(nullTuple));
        }

        [Test] public void TestValueTuple()
        {
            var stringComposite = new DynamicTuple(new string[] { "a", "b" });
            var stringComposite2 = new ValueTuple<string, string>( "a", "b" );
            Assert.AreEqual(stringComposite, stringComposite2);

            var intStringComposite = new DynamicTuple(new object[] { 12, "b" });
            var intString2 = new ValueTuple<int, string>( 12, "b");
            Assert.IsTrue(intStringComposite.Equals(intString2));
        }

        [Test]
        public void TestTuple()
        {
            var stringComposite = new DynamicTuple(new string[] { "a", "b" });
            var stringComposite2 = new Tuple<string, string>("a", "b");
            Assert.AreEqual(stringComposite, stringComposite2); //these 3 lines are testing different things
            Assert.IsTrue(stringComposite == stringComposite2);
            Assert.IsTrue( stringComposite2 == stringComposite);

            var intStringComposite = new DynamicTuple(new object[] { 12, "b" });
            var intString2 = new Tuple<int, string>(12, "b");
            Assert.IsTrue(intStringComposite.Equals(intString2));

            var intString3 = (Tuple<int, string>)null;
            Assert.IsFalse(intStringComposite.Equals(intString3));
        }
    }
}
