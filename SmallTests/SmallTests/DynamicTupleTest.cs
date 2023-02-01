using SmallTests.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Data;

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

        [Test] public void TestValueTupleEqualsDynamicTuple()
        {
            var dynamicTuple = new DynamicTuple(new string[] { "a", "b" });
            var valueTuple = new ValueTuple<string, string>( "a", "b" );

            //these 5 lines are testing different things
            Assert.AreEqual(dynamicTuple, valueTuple); //call .Equals()
            Assert.IsTrue(dynamicTuple.Equals(valueTuple));  //I implemented it in DynamicTuple
            Assert.IsFalse(valueTuple.Equals(dynamicTuple));   // ValueTuple first check if type match, this is different than Tuple
            Assert.IsTrue(dynamicTuple == valueTuple);  //call public static bool operator ==(DynamicTuple x, ITuple y)
            Assert.IsTrue(valueTuple == dynamicTuple); //call public static bool operator ==(ITuple y, DynamicTuple x)


            var dynamicTuple2 = new DynamicTuple(new object[] { 12, "b" });
            var valueTuple2 = new ValueTuple<int, string>( 12, "b");
            //these 5 lines are testing different things
            Assert.AreEqual(dynamicTuple2, valueTuple2); //call .Equals()
            Assert.IsTrue(dynamicTuple2.Equals(valueTuple2));  //I implemented it in DynamicTuple
            Assert.IsFalse(valueTuple2.Equals(dynamicTuple2));   // ValueTuple first check if type match, this is different than Tuple
            Assert.IsTrue(dynamicTuple2 == valueTuple2);  //call public static bool operator ==(DynamicTuple x, ITuple y)
            Assert.IsTrue(valueTuple2 == dynamicTuple2); //call public static bool operator ==(ITuple y, DynamicTuple x)
        }

        [Test]
        public void TestTupleEqualDynamicTuple()
        {
            var dynamicTuple = new DynamicTuple(new string[] { "a", "b" });
            var tuple = new Tuple<string, string>("a", "b");

            //these 5 lines are testing different things
            Assert.AreEqual(dynamicTuple, tuple); //call .Equals()
            Assert.IsTrue(dynamicTuple.Equals(tuple));  //I implemented it in DynamicTuple
            Assert.IsFalse(tuple.Equals(dynamicTuple));   // ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
            Assert.IsTrue(dynamicTuple == tuple);  //call public static bool operator ==(DynamicTuple x, ITuple y)
            Assert.IsTrue(tuple == dynamicTuple); //call public static bool operator ==(ITuple y, DynamicTuple x)


            var dynamicTuple2 = new DynamicTuple(new object[] { 12, "b" });
            var tuple2 = new Tuple<int, string>(12, "b");
            //these 5 lines are testing different things
            Assert.AreEqual(dynamicTuple2, tuple2); //call .Equals()
            Assert.IsTrue(dynamicTuple2.Equals(tuple2));  //I implemented it in DynamicTuple
            Assert.IsFalse(tuple2.Equals(dynamicTuple2));   // ValueTuple first check if type match, this is different than Tuple
            Assert.IsTrue(dynamicTuple2 == tuple2);  //call public static bool operator ==(DynamicTuple x, ITuple y)
            Assert.IsTrue(tuple2 == dynamicTuple2); //call public static bool operator ==(ITuple y, DynamicTuple x)            
        }

        [Test]
        public void TestEqualCheckWithNullITupleWontCauseException()
        {
            var intStringComposite = new DynamicTuple(new object[] { 12, "b" });
            //just check if null cause exception
            var intString3 = (Tuple<int, string>)null;
            Assert.IsFalse(intStringComposite.Equals(intString3));

        }
    }
}
