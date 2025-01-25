using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Constraints;
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
            Assert.IsTrue(a.Equals(b));

            Assert.IsFalse(a == b);
            Assert.IsFalse(object.ReferenceEquals(a, b));
        }

        [Test]
        public void TestValueTupleEqualByItemsAndByRef()
        {
            var a = ValueTuple.Create(1, "a");
            var b =(1, "a");
            Assert.AreEqual(a, b);
            Assert.IsTrue(a == b);
            
        }

        [Test]
        public void EquateTwoValueTuplesWithSameContent1()
        {
            var t1 = ("A1", 1);
            var t2 = ValueTuple.Create(t1.Item1,1);
            Assert.IsTrue(t1.Equals(t2));
            Assert.IsTrue(t1 == t2);  
            
        }

        [Test]
        public void EquateTwoValueTuplesWithSameContent2() // !!!!!!!!!!!!!!!!!
        {
            var t1 = ValueTuple.Create("A1");
            var t2 = ValueTuple.Create("A1");
            Assert.IsTrue(t1.Equals(t2));

            //Error CS0019  Operator '==' cannot be applied to operands of type 'ValueTuple<string>' and 'ValueTuple<string>' 
            //Assert.IsTrue(t1 == t2);  //!!! won't even compile even same type VauleTuple<string> or VauleTuple<int>, but works for VauleTuple<string, int>

            Assert.IsFalse(Object.ReferenceEquals(t1, t2));//Due to boxing, this test is not meaningful, it will always return false
        }


        [Test]
        public void EquateTwoValueTuplesWithSameContent3() 
        {
            var t1 = ValueTuple.Create("A1", "A1");
            var t2 = ValueTuple.Create("A1", "A1");

            Assert.IsTrue(t1 == t2);  // right
        }

        [Test]
        public void EquateTwoValueTuplesWithSameContent4() 
        {
            var t1 = ValueTuple.Create(1);
            var t2 = ValueTuple.Create(1);

            //Assert.IsTrue(t1 == t2);  // won't compile
        }



        //--------
        [Test]
        public void EquateTwoTuplesWithSameContent()
        {
            var t1 = Tuple.Create("S");
            var t2 = Tuple.Create(t1.Item1);
            Assert.IsTrue(t1.Equals(t2));
            Assert.IsFalse(t1 == t2);  //!!!
            Assert.IsFalse(Object.ReferenceEquals(t1, t2));
        }

        [Test]
        public void EquateTwoTuplesWithSameContentDifferentType()
        {
            var t1 = Tuple.Create("S");
            var t2 = Tuple.Create((object)t1.Item1);
            Assert.IsFalse(t1.Equals(t2));
            // Assert.IsFalse(t1 == t2); // Won't even compile, since they are different type
            Assert.IsFalse(Object.ReferenceEquals(t1, t2));

        }

        [Test]
        public void EquateTwoValueTuplesWithSameContent()
        {
            var t1 = ValueTuple.Create("S");
            var t2 = ValueTuple.Create(t1.Item1);
            Assert.IsTrue(t1.Equals(t2));
            // Assert.IsFalse(t1 == t2);  //!!! won't even compile even same type, worked with Tuple<T> or ValueTuple<T1, T2>
            Assert.IsFalse(Object.ReferenceEquals(t1, t2));//does not make sense since there is boxing
        }

        [Test]
        public void EquateTwoValueTuplesWithSameContentDifferentType()
        {
            var t1 = ValueTuple.Create("S");
            var t2 = ValueTuple.Create((object)t1.Item1);
            Assert.IsFalse(t1.Equals(t2));
            // Assert.IsFalse(t1 == t2); // Won't even compare, since different type            

        }
        //==========








        [Test]
        public void TestString()
        {
            var t1 = ("A1");
            string t2 = ("A1"); //not ValueTuple, but string
            Assert.IsTrue(t1.Equals(t2));
            Assert.IsTrue(t1 == t2); 
            Assert.IsTrue(Object.ReferenceEquals(t1, t2));
            Assert.IsTrue(String.IsInterned(t1) != null); // Yes, it is intered, what is why reference equals.
            Assert.IsTrue(String.IsInterned(t2) != null);
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

        [Test]
        public void AnonymouysTypeAreEqual()
        {
            var a = new { A = "A" };
            var b = new { A = "A" };
            Assert.IsTrue(a.Equals(b));
           
            Assert.IsFalse(a == b);
            Assert.IsFalse(Object.ReferenceEquals(a, b));

        }

    }
}
