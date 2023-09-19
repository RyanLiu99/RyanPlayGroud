using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAppFramework
{
    //Prove each thread starts an "asynclocal storage", even no await used. 

    public class Holder
    {
        private  AsyncLocal<string> _localAsync = new AsyncLocal<string>();  //static or not does not make difference since Holder is singleton in test
        
         private string _local;

         public string ValueLocalAsync
         {
             get => _localAsync.Value;
             set => _localAsync.Value = value;
         }

        public string Value
        {
            get => _local;
            set => _local = value;
        }

        public void Print()
        {
          Console.WriteLine($"Current Thread {Thread.CurrentThread.Name}, AsyncLocal var : {ValueLocalAsync}; Simple var : {Value}.");
        }
    }

    internal class TestAsyncLocal
    {
        private readonly Holder _holder = new Holder(); //reused singleton instance

        internal void Test()
        {
            Enumerable.Range(1, 100).Select(x =>
                {
                    var t = new Thread(() => { this.DoSth(x.ToString()); }) { Name = "T" + x };
                    t.Start();
                    return t;
                }
            ).ToArray();

        }

        private void DoSth(string input)
        {
            _holder.Value = input;
            _holder.ValueLocalAsync = input;
            // if sleep and use regular var, all print value will be 100. NO sleep, right value.
            // if _local is string, not int, value all stepping each other, including  "-----x" when sleeping
            Thread.Sleep(1); 
            _holder.Print();

            _holder.Value = "--------" + input;
            _holder.ValueLocalAsync = "********" + input;
        }
    }
}
