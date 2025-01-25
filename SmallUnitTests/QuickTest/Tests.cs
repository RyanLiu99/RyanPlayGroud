using AutoFixture;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuickTest
{
    internal class Tests
    {
        Fixture fixture = new Fixture();

        [Test]
        public void MyTest()
        {
            var data = fixture.Build<ValueTuple<int, string>>()
                .With(x => x.Item1, 0)
                .CreateMany(20).ToArray();
            
            Console.WriteLine($"============={data.Length}");
            Assert.IsNotNull(data);
        }
    }
}
