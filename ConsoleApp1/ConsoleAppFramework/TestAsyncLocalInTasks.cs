using System;
using System.CodeDom;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAppFramework
{

    //Value set before thread starts, is used by each thread 
    //Then each thread operate on value independently. 

    internal class TestAsyncLocalInTasks
    {
        public string Name { get; private set; }
        private static readonly AsyncLocal<int> LocalAsync = TestAsyncLocalInThreads.LocalAsync; // //new AsyncLocal<int>();

        private Random r = new Random();

        public TestAsyncLocalInTasks(string name)
        {
            Name = name;
        }
        public void Test()
        {
            LocalAsync.Value = 5000;
            Task t1 = new Task(DoSth);
            var t2 = Task.Run(DoSth);
            t1.Start();

            t2.ContinueWith((_ ) => DoSth());
            
        }


        private void DoSth()
        {
            for(int i =0; i <10; i++)
            {
                Thread.Sleep(r.Next(20));
                Console.WriteLine( $"{Name}, {Thread.CurrentThread.Name}, {i} -- {LocalAsync.Value++}");
            }
        }

    }
}