using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ConsoleApp1
{
    internal static class TestDateTime
    {
        private const string format = "yyyy-MM-dd HH:mm:ss z";
        public static void Test()
        {
            DateTime dt1 = DateTime.Now;
            Thread.Sleep(TimeSpan.FromMinutes(3));
            DateTime dt2 = DateTime.Now;
            Console.WriteLine($"{dt1.ToString(format)}, {dt2.ToString(format)}, Diff in minutes: {(dt2-dt1).TotalMinutes}.");

        }
        //Normally:                          2021-06-02 11:17:09 -7, 2021-06-02 11:20:09 -7, Diff in minutes: 3.000290595.
        //ON the day daylight saving switch: 2021-11-07 01:58:32 -7, 2021-11-07 01:01:32 -8, Diff in minutes: -56.999575236666665.
    }
}
