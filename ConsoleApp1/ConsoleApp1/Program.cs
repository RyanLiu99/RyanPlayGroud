using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {            
            var numbers = GetEnumberable();           
            var c = numbers.Count();
            Console.WriteLine("Total: {0}", c);

            Console.WriteLine("Going to call any");
            if (numbers.Any())
            {
                Console.WriteLine("Any true");
            }
        }


        public static IEnumerable<int> GetEnumberable(int count = 1)
        {

            while (count-- > 0)
            {
                Console.WriteLine("Getting {0}", count);
                yield return count;
            }
        }
    }
}
/*
 * Ryan:  Don’t return IEnumerable<> unless you have to
 * 
Output:  
Getting 0              <<<<<<<<< method called by Count()    
Total: 1
Going to call any
Getting 0               <<<<<<< Method Called again by Any() !!!
Any true

 */


