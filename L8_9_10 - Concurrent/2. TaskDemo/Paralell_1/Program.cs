using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ParallelTest
{
    public class TestParallel
    {
        // Simulates a time-consuming operation (1 second delay)
        public int TestOperation(int data)
        {
            Thread.Sleep(1000); // Simulated workload
            return data;
        }
    }

    // Synchronous operation. Result: 210. Time: 20226 ms

    internal class Program
    {
        private static void SyncMethod()
        {
            // Generate an array of integers from 1 to 20
            int[] dataArray = Enumerable.Range(1, 20).ToArray();

            int result = 0;

            // Create stopwatch to measure execution time
            Stopwatch watch = new Stopwatch();
            watch.Start();

            // Process each item in the array sequentially
            foreach (int data in dataArray)
            {
                TestParallel parallel = new TestParallel();
                result += parallel.TestOperation(data);
            }

            watch.Stop();

            // Output the result and the time taken
            Console.WriteLine("Synchronous operation. Result: {0}. Time: {1} ms", result, watch.ElapsedMilliseconds);
        }

        private static void Main(string[] args)
        {
            SyncMethod();
            Console.ReadLine();
        }
    }
}