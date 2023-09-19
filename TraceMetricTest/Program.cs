using System;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;

class Program
{
    private static Meter m = new Meter("TraceMetricTest");
    private static Counter<int> _count1 = m.CreateCounter<int>("Count 1");
    static void Main()
    {
        _count1.Add(1);
        

        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }


    private static void Loop()
    {
        m.
        while (true)
        {

        }

    }
}