using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SmallTests
{
    [TestClass]
    public class SpanMemoryTTest
    {
        [TestMethod]
        public void TestSpanT()
        {
            var arr = new byte[10];
            Span<byte> bytes = arr; // Implicit cast from T[] to Span<T>

            bytes[2] = 45; // OK
            Assert.AreEqual(arr[2], bytes[2]);
            Assert.AreEqual(45, arr[2]);


            Span<byte> slicedBytes = bytes.Slice(start: 5, length: 2);
            byte
                x = slicedBytes[
                    0]; //The indexer is declared with a “ref T” return type, which provides semantics like that of indexing into arrays, returning a reference to the actual storage location 
            slicedBytes[0] = 42;
            slicedBytes[1] = 43;
            Assert.AreEqual(42, slicedBytes[0]);
            Assert.AreEqual(43, slicedBytes[1]);
            Assert.AreEqual(arr[5], slicedBytes[0]);
            Assert.AreEqual(arr[6], slicedBytes[1]);
            //slicedBytes[2] = 44; // Throws IndexOutOfRangeException

        }

        [TestMethod]

        public void TestString()
        {
            string str = "hello, world";
            //string worldString = str.Substring(startIndex: 7, length: 5); // Allocates

            ReadOnlySpan<char> worldSpan = str.AsSpan().Slice(start: 7, length: 5); // No allocation
            Assert.AreEqual('w', worldSpan[0]);
            //worldSpan[0] = 'a'; // Compile error CS8331: Cannot assign to because it is readonly variable
        }

        struct MutableStruct
        {
            public int Value;
        }

        [TestMethod]
        public void TestIndexer()
        {
            Span<MutableStruct> spanOfStructs = new MutableStruct[1];
            spanOfStructs[0].Value = 42; //OK
            Assert.AreEqual(42, spanOfStructs[0].Value);

            var listOfStructs = new List<MutableStruct> { new MutableStruct() };
            //listOfStructs[0].Value = 42; // Compile Error CS1612: the return value is not a variable

            MutableStruct x = listOfStructs[0]; //this is ok, but it another copy
            x.Value = 43;
        }

        public static void AddOne(ref int value) => value += 1;

        [TestMethod]
        public void TestOldRefParam()
        {
            var values = new int[] { 42, 84, 126 };
            AddOne(ref values[2]);
            Assert.AreEqual(127, values[2]);
        }

        [TestMethod]
        public void TestMemoryT()
        {
            
            string s = "123";
            foreach (var x in GetChunks(s, 20))
            {
                Console.WriteLine(x);
            }
        }

        private IEnumerable<ReadOnlyMemory<char>> GetChunks(string input, int length)
        {
            if (length <= 0)
                throw new ArgumentOutOfRangeException(nameof(length), length, "Length must greater than 0");

            if(string.IsNullOrEmpty(input)) yield break;

            int time = 0;
            int totalLength = input.Length;
            int times = totalLength / length;
            int remain = totalLength % length;

            Console.WriteLine($"Times : {times}, Remain {remain}");
            var spans = input.AsMemory();
            
            while ( time  < times)
            {
                var result = spans.Slice(time * length, length);
                time++;
                yield return result;
            }

            if (remain > 0)
            {
                yield return spans.Slice(totalLength - remain, remain);
            }

        }
    }
}

