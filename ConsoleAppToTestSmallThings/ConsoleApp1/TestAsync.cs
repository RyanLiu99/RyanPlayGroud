using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal static class TestAsync
    {
        internal static async Task M1()
        {
            PrintThreadInfo("M1", true);
            await M1_1().ConfigureAwait(false);
            PrintThreadInfo("M1", false);
        }

        internal static async Task M1_1()
        {
            PrintThreadInfo("M1_1", true);
            await Task.Run(() => M1_1_1());
            //await M1_1_1().ConfigureAwait(false);
            PrintThreadInfo("M1_1", false);
        }

        internal static async Task M1_1_1()
        {
            for (int i = 0; i < 3; i++)
            {
                PrintThreadInfo("M1_1_1:" + i, true);
                await Task.Delay(3).ConfigureAwait(false);
                PrintThreadInfo("M1_1_1:" + i, false);
            }
        }

        internal static void PrintThreadInfo(string method, bool before)
        {
            var beforeStr = before ? "before" : "after";
            Console.WriteLine($"Thread Id: {Thread.CurrentThread.ManagedThreadId} in {method} {beforeStr} await calls");
        }
    }
}
