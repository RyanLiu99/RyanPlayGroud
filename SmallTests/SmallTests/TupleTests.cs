using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

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
    }
}
