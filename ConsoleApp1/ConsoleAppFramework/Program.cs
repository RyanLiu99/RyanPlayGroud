using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppFramework
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("ConsoleAppFramework start...");
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            try
            {
                //  new TestAsyncLocal().Test();

                await new TestAsyncLambdaReturnTypeAndException().TestTask();
                new TestAsyncLambdaReturnTypeAndException().TestAsyncVoid();
            }
            catch (Exception e)
            {
                Console.WriteLine("Main caught exception:" + e);
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine("CurrentDomain_UnhandledException: " + e.ToString());
            
        }
    }
}
