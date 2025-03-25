
using System.ComponentModel;

namespace Application
{
    class Application
    {
        public static void Main()
        {
            long[] ITERATIONS = { 100000, 1000000, 200000, 3000000, 400000,
                                  100000, 1000000, 200000, 3000000, 400000 };

            List<BackgroundWorker> executors = new List<BackgroundWorker>();

            for (int i = 0; i < 10; i++)
            {
                BackgroundWorker w = new BackgroundWorker();

                w.WorkerSupportsCancellation = true;
                w.WorkerReportsProgress = false;

                long j = i;
                w.DoWork += (o, e) =>
                {
                    Console.WriteLine("Running worker: " + j);

                    long iterationsCount = ITERATIONS[j];      // why we can't use i here ?
                    BackgroundWorker sender = o as BackgroundWorker;

                    decimal res = 0;
                    for (long i = 0; i < iterationsCount; i++)
                    {
                        if ((i & 0x3FFFF) == 0)
                        {
                            if (w.CancellationPending) {
                                e.Cancel = true;
                                break;
                            }
                        }

                        res += (decimal) Math.Sin(i);
                    }

                    e.Result = res;
                };

                w.RunWorkerCompleted += (o, e) =>
                {
                    Console.WriteLine(e.Cancelled
                        ? "Cancelled!"
                        : "Result=" + e.Result.ToString());
                };

                executors.Add(w);
            }

            foreach (BackgroundWorker w in executors)
            {
                Console.WriteLine("Running worker...");
                w.RunWorkerAsync();// .....
            }

            Console.ReadLine();
        }
    }
}
