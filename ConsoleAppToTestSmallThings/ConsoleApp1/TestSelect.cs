using ConsoleApp1.TestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    //array.ToArrary(）or list<T>.ToList() both will return a new array or list, not the same instance. 
    //IEnumerable<T>.Any() will not enumerate one more time, but foreach or.Count() each will enumerate once more.

    internal static class TestSelect
    {
        public static void TestSelectMethod()
        {

            var addresses = GetAddress();
            Console.WriteLine("Count of addresses " + addresses.Count());

            if (addresses.Any())
            {
                foreach (var add in addresses)
                {
                    Console.WriteLine(add.StrNo);
                }
            }

        }
        public static IEnumerable<Address> GetAddress()
        {

            var ints = Enumerable.Range(1, 3).ToArray();
            var list = ints.ToArray();
            Console.WriteLine("list/array to list/array is self " + (list == ints));

            ints[0] = 50;
            list[1] = 60;
            //ints.Add(10);
            //list.Add(20);

            return ints.Select((i) =>
            {
                Console.WriteLine("Build addr " + i);
                var add = new Address() { StrNo = i };
                return add;
            });
        }
    }
}
