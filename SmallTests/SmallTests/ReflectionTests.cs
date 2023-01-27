using NUnit.Framework;
using SmallTests.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallTests
{
    internal class ReflectionTests
    {

        [Test]
        public void TestReflection()
        {
            Assert.IsTrue(typeof(EntityDependency) == (typeof(EntityDependency)));
            Assert.IsFalse(typeof(EntityDependency).IsSubclassOf(typeof(EntityDependency)));
            Assert.IsTrue(typeof(EntityDependency<>).IsSubclassOf(typeof(EntityDependency)));
            Assert.IsTrue(typeof(EntityDependency<,>).IsSubclassOf(typeof(EntityDependency)));

            decimal d = 2.23m;
            Assert.IsFalse(typeof(decimal).IsPrimitive);
            Assert.IsTrue(d.GetType() == typeof(decimal));
        }


    }
}
