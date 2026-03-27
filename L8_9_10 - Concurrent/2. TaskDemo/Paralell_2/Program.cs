using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

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

    // Asynchronous operation. Result: 210. Time: 1083 ms

    internal class Program
    {
        // Shared lock object for synchronization
        private static readonly object _key = new object();

        private static void AsyncMethod()
        {
            int[] dataArray = Enumerable.Range(1, 20).ToArray();
            int arrayCount = dataArray.Length;
            int sum = 0;
            int i = 0;

            Stopwatch watch = new Stopwatch();
            watch.Start();

            foreach (int data in dataArray)
            {
                Thread thread = new Thread(() =>
                {
                    TestParallel parallel = new TestParallel();
                    int result = parallel.TestOperation(data);

                    lock (_key)
                    {
                        sum += result;
                        i += 1;
                    }
                });

                thread.Start();
            }

            // Wait until all threads are done
            while (true)
            {
                lock (_key)
                {
                    if (i >= arrayCount)
                        break;
                }
                Thread.Sleep(100); // Prevent busy-waiting
            }

            watch.Stop();
            Console.WriteLine("Asynchronous operation. Result: {0}. Time: {1} ms", sum, watch.ElapsedMilliseconds);
        }

        private static void Main(string[] args)
        {
            AsyncMethod();
            Console.ReadLine();
        }
    }
}