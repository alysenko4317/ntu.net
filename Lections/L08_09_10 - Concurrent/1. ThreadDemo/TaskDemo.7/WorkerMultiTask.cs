using System;
using System.Threading;

namespace MultiThreadTest
{
    public class WorkerMultiTask
    {
        private readonly int _workId;

        public WorkerMultiTask(int workId)
        {
            _workId = workId;
        }

        public int Work(dynamic obj)
        {
            int delay = obj.Delay;
            CancellationToken token = obj.Token;

            for (int i = 0; i <= 99; i++)
            {
                token.ThrowIfCancellationRequested();

                Thread.Sleep(delay);

                if (i == 30)
                    throw new Exception("Щось пішло не за планом!");

                ProcessChanged(i, _workId);
            }

            return delay;
        }

        public event Action<int, int> ProcessChanged;
    }
}