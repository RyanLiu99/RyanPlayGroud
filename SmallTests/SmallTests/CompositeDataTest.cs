using SmallTests.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SmallTests
{
    internal class CompositeDataTest
    {
        [Test]
        public void TestEq()
        {
            var stringComposite = new CompositeData(new string[] { "a", "b" });
            var stringComposite2 = new CompositeData(new List<string> { "a", "b" });
            Assert.AreEqual(stringComposite, stringComposite2);

            var intStringComposite = new CompositeData(new object[] { 12, "b" });
            var intString2 = new CompositeData(new List<object> { 12, "b" });
            Assert.IsTrue(intStringComposite.Equals( intString2));
        }

        [Test]
        public void TestNotEq()
        {
            var stringComposite = new CompositeData(new string[] { "a", "b" });
            var stringComposite2 = new CompositeData(new List<string> { "a1", "b" });
            Assert.AreNotEqual(stringComposite, stringComposite2);
        }
    }
}
