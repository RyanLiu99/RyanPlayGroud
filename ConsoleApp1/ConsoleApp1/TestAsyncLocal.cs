using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Holder
    {
        private AsyncLocal<string> _local = new AsyncLocal<string>();

        public string Value
        {
            get => _local.Value;
            set => _local.Value = value;
        }

        public void Print()
        {
          Console.WriteLine($"Current Thread {Thread.CurrentThread.Name}, Value : {Value}");
        }
    }

    internal class TestAsyncLocal
    {
        private Holder holder = new Holder();

        internal void Test()
        {
           var threads = Enumerable.Range(1, 100).Select(x =>
                new Thread(() => { this.DoSth(x.ToString()); }) { Name = "T" + x });

           Parallel.ForEach(threads, t => t.Start());
        }

        private void DoSth(string input)
        {
            holder.Value = input;
            holder.Print();
            holder.Value = "--------" + input;
        }
    }
}
