using System;

using System.Threading;

namespace ConsoleAppFramework
{

    //Value set before thread starts, is used by each thread 
    //Then each thread operate on value independently. 

    internal class TestAsyncLocalInThreads
    {
        public string Name { get; private set; }
        internal static readonly AsyncLocal<int> LocalAsync =  new AsyncLocal<int>();

        private Random r = new Random();
        Thread t1;
        Thread t2;

        public TestAsyncLocalInThreads(string name)
        {
            Name = name;
        }
        public void Test()
        {
            LocalAsync.Value = 100;
            
            t1 = new Thread(DoSth){ Name = "T1"};
            t2 = new Thread(DoSth) { Name = "T2" }; 

            t1.Start();
            t2.Start();
          
        }

        public void AssureEnd()
        {
            t1.Join();
            t2.Join();
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