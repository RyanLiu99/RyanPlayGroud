using System;
using System.Threading;
using System.Threading.Tasks;

public class TestTaskRun
{
    public static async Task MainTask()
    {
        BackendService service = new BackendService();

        // Example usage
        await service.ProcessDataAsync();
        await service.HandleLongRunningTaskAsync();
        await service.ProcessMultipleTasksAsync();
    }
}

public class BackendService
{

    public static void PrintThredId(string where)
    {
        Console.WriteLine($"{DateTime.Now} {where} - {Thread.CurrentThread.ManagedThreadId}");
    }

    // Simulate a method that performs a CPU-bound operation
    public async Task ProcessDataAsync()
    {
        PrintThredId(nameof(ProcessDataAsync));
        // Offload CPU-bound work to the thread pool
        await Task.Run(async () =>
        {
            PrintThredId("Inside Task Run of " + nameof(ProcessDataAsync));
            ComputeIntensiveOperation();
            await Task.Delay(1000); // Simulate some additional async work
            Console.WriteLine("CPU-bound operation completed.");
        });
    }

    // Simulate a CPU-bound operation
    private void ComputeIntensiveOperation()
    {
        int result = 0;
        for (int i = 0; i < 1000000; i++)
        {
            result += i;
        }
    }

    // Simulate a method that performs a long-running synchronous operation
    public async Task HandleLongRunningTaskAsync()
    {
        await Task.Run(async () =>
        {
            LongRunningOperation();
            await Task.Delay(500); // Simulate some additional async work
            Console.WriteLine("Long-running operation completed.");
        });
    }

    // Simulate a long-running synchronous operation
    private void LongRunningOperation()
    {
        Thread.Sleep(5000); // Simulate long-running work
    }

    // Example method to process multiple tasks in parallel
    public async Task ProcessMultipleTasksAsync()
    {
        var task1 = Task.Run(async () => await DoWork1Async());
        var task2 = Task.Run(async () => await DoWork2Async());

        await Task.WhenAll(task1, task2);
    }

    private async Task DoWork1Async()
    {
        await Task.Delay(1000); // Simulate I/O-bound work
        Console.WriteLine("Work 1 completed.");
    }

    private async Task DoWork2Async()
    {
        await Task.Delay(1500); // Simulate I/O-bound work
        Console.WriteLine("Work 2 completed.");
    }
}

