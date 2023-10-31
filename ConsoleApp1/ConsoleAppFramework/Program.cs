using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAppFramework
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("ConsoleAppFramework start...");

            //  new TestAsyncLocal().Test();

            //  await TestException();

            var t1 = new TestAsyncLocalInThreads("Test1");
                t1.Test();
            var t2 = new TestAsyncLocalInThreads("Test2");
                t2.Test();

            new TestAsyncLocalInTasks("Task1").Test();

            t1.AssureEnd();
            t2.AssureEnd();
            Console.WriteLine( $"==== AsyncLocal value in Main is {TestAsyncLocalInThreads.LocalAsync.Value} sine it was set in main thread");

        }

        private static async Task TestException()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException; //does not work, caught nothing
            Process.GetCurrentProcess().ErrorDataReceived += Program_ErrorDataReceived; //does not work, caught nothing
            try
            {
                await new TestAsyncLambdaReturnTypeAndException().TestTask();
                new TestAsyncLambdaReturnTypeAndException().TestAsyncVoid();
            }
            catch (Exception e)
            {
                Console.WriteLine("Main caught exception:" + e.Message);
            }

            void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
            {
                Console.WriteLine("CurrentDomain____UnhandledException: " + e.ToString());
            }

            void Program_ErrorDataReceived(object sender, DataReceivedEventArgs e)
            {
                Console.WriteLine("Program____ErrorDataReceived: " + e.ToString());
            }
        }

        
    }
}
