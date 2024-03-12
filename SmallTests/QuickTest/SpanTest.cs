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
    internal class SpanTest
    {
        [Test]
        public void TestSpan()
        {
            var space = ' ';
            var seperator = new ReadOnlySpan<char>(new[] { space });

            ReadOnlySpan<char> span = "Hello, world!".AsMemory().Span;
            Span<Range> destination = new Span<Range>(new Range[5]);
            int count = span.Split(destination, seperator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            for (int c = count - 1; c >= 0; c--)
            {
                var range = destination[c];
                for (var pos = range.Start.Value; pos < range.End.Value; pos++)
                {
                    Debug.Write(span[pos]);
                }
                Debug.Write(space);
            }
        }
        

        [Test]
        public void TestValueTuple()
        {
            var a = 10;
            var b = 20;
            (a, b) = (b, a);
            Assert.That(b, Is.EqualTo(10));
            Assert.That(a, Is.EqualTo(20));
        }
    }
}
