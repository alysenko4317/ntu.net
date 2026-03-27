using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    private static int _sharedResource = 0;

    static void Main()
    {
        // Create a list of tasks

        var tasks = new List<Task>();
        for (int i = 0; i < 100; i++) {
            tasks.Add(Task.Run(() => IncrementResource()));
            tasks.Add(Task.Run(() => DecrementResource()));
        }

        Task.WhenAll(tasks).Wait();  // Wait for all tasks to complete

        Console.WriteLine($"Final value of shared resource: {_sharedResource}");
    }

    static void IncrementResource()
    {
        for (int i = 0; i < 1000; i++)
            _sharedResource++;
    }

    static void DecrementResource()
    {
        for (int i = 0; i < 1000; i++)
            _sharedResource--;
    }
}
