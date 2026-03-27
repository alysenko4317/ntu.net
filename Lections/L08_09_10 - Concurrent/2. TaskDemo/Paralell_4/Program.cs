using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelTest
{
    public class TestParallel
    {
        // Simulated workload
        public int TestOperation(int data)
        {
            Thread.Sleep(1000); // Heavy operation
            return data;
        }
    }

    internal class Program
    {
        private static readonly object _key = new object();

        private static void ParallelMethod()
        {
            int[] dataArray = Enumerable.Range(1, 20).ToArray();
            int sum = 0;

            Stopwatch watch = new Stopwatch();
            watch.Start();

            // Set up cancellation token
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            ParallelOptions options = new ParallelOptions();
            options.CancellationToken = tokenSource.Token;

            // Start background task to monitor for cancellation
            Task.Factory.StartNew(() =>
            {
                if (Console.ReadKey().KeyChar == '1')
                    tokenSource.Cancel();
            });

            bool cancel = false;

            try
            {
                Parallel.ForEach(dataArray, options, data =>
                {
                    TestParallel parallel = new TestParallel();
                    int result = parallel.TestOperation(data);

                    // Check for cancellation inside the loop
                    options.CancellationToken.ThrowIfCancellationRequested();

                    lock (_key)
                    {
                        sum += result;
                    }
                });
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine();
                Console.WriteLine("Operation canceled.");
                cancel = true;
            }

            watch.Stop();

            if (!cancel)
            {
                Console.WriteLine("Parallel operation completed. Result: {0}. Time: {1} ms", sum, watch.ElapsedMilliseconds);
            }
        }

        private static void Main(string[] args)
        {
            Console.WriteLine("Press '1' to cancel the operation during execution...");
            ParallelMethod();
            Console.ReadLine();
        }
    }
}
