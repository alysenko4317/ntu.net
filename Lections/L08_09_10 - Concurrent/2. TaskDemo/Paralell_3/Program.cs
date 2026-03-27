using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelTest
{
    public class TestParallel
    {
        // Simulates a time-consuming operation
        public int TestOperation(int data)
        {
            Thread.Sleep(1000); // Simulated workload
            return data;
        }
    }

    // Parallel operation. Result: 210. Time: 2957 ms

    internal class Program
    {
        // Shared lock object for synchronization
        private static readonly object _key = new object();

        private static void ParallelMethod()
        {
            int[] dataArray = Enumerable.Range(1, 20).ToArray();
            int sum = 0;

            Stopwatch watch = new Stopwatch();
            watch.Start();

            // Run in parallel using Parallel.ForEach
            Parallel.ForEach(dataArray, data =>
            {
                TestParallel parallel = new TestParallel();
                int result = parallel.TestOperation(data);

                // Safely update shared result
                lock (_key)
                {
                    sum += result;
                }
            });

            watch.Stop();

            Console.WriteLine("Parallel operation. Result: {0}. Time: {1} ms", sum, watch.ElapsedMilliseconds);
        }

        private static void Main(string[] args)
        {
            ParallelMethod();
            Console.ReadLine();
        }
    }
}