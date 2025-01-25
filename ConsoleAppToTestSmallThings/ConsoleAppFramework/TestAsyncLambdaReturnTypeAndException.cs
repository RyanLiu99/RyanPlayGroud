using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppFramework
{
    //When it runs, no exception will ever be printed or even caught, not in this class, not in main.
    //when async, there is no await, async will be ignored. NO task will be crated, all return void at run time. -- RL
    //but even with await, no exception ever logged anywhere!!!  Because nothing awaited?
    internal class TestAsyncLambdaReturnTypeAndException
    {
        public void TestAsyncVoid()
        {
            try
            {
                RunVoidAsync(async () =>
                {
                    Console.WriteLine("RunVoidAsync starts ...");
                    await Task.Delay(1);
                    throw new Exception("RunVoidAsync ex");
                });
            }
            catch (Exception e)
            {
                Console.WriteLine("RunVoidAsync exception caught: " +  e.Message);
                
            }
        }

        //throw new Exception("async void action called")
        public async Task TestTask()
        {
            try
            {
                await RunTask(async () =>
                {
                    Console.WriteLine("RunTask starts...");
                    await Task.Delay(1);
                    throw new Exception("RunTask Ex");
                });
            }
            catch (Exception e)
            {
                Console.WriteLine("RunTask exception caught: " + e.Message);
                

            }
        }

        //same lambda can be used in different calls
        private void RunVoidAsync(Action doSomeThing)
        {
            doSomeThing();
        }

        private async Task RunTask(Func<Task> doSomeThing)
        {
             await doSomeThing();
             Console.WriteLine("after await in RunTask"); //this never printed
        }
    }
}
