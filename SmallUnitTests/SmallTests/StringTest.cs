using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SmallTests.Entities;

namespace SmallTests
{
    internal class StringTests
    {
        [Test]
        public void StringIsIntered()
        {
            string b = "B";
            var ab = $"A {b}";
            
            Assert.IsTrue(string.IsInterned("c") != null);
            
            Assert.IsTrue(string.IsInterned(b) != null);

            Assert.IsTrue(string.IsInterned(ab) == null); //dynamic string is not intered

        }

     
    }
}
