using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace ConsoleAppNet48
{
    class Program
    {
        static void Main(string[] args)
        {
            CancellationChangeToken token = new CancellationChangeToken(new CancellationToken());
        }
    }
}
