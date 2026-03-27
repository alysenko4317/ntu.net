using System;
using System.Threading;

namespace MultiThreadTest
{
    public class WorkerRx
    {
        public bool Cancelled { get; private set; }

        public void Cancel()
        {
            Cancelled = true;
        }

        public IEnumerable<int> Work()
        {
            for (int i = 0; i <= 100; i++)
            {
                if (Cancelled)
                    break;

                Thread.Sleep(50);

                yield return i;
            }
        }
    }
}