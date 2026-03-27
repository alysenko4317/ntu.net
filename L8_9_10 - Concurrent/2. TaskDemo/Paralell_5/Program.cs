using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelTest
{
    public class TestParallel
    {
        // Simulate workload
        public int TestOperation(int data)
        {
            Thread.Sleep(1000); // Simulated heavy work
            return data;
        }
    }

    internal class Program
    {
        private static void LinqMethod()
        {
            int[] dataArray = Enumerable.Range(1, 20).ToArray();
            Stopwatch watch = new Stopwatch();

            // Create cancellation token
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;

            // Background task to listen for '1' key and cancel
            Task.Factory.StartNew(() =>
            {
                if (Console.ReadKey().KeyChar == '1')
                    tokenSource.Cancel();
            });

            int sum = 0;
            bool canceled = false;

            watch.Start();
            try
            {
                // Use PLINQ with cancellation
                sum = dataArray
                    .AsParallel()
                    .WithCancellation(token)
                    .Sum(data =>
                    {
                        TestParallel parallel = new TestParallel();
                        return parallel.TestOperation(data);
                    });
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine();
                Console.WriteLine("Operation was canceled.");
                canceled = true;
            }
            watch.Stop();

            if (!canceled)
            {
                Console.WriteLine("Parallel LINQ operation. Result: {0}. Time: {1} ms", sum, watch.ElapsedMilliseconds);
            }
        }

        private static void Main(string[] args)
        {
            Console.WriteLine("Press '1' to cancel the operation...");
            LinqMethod();
            Console.ReadLine();
        }
    }
}
