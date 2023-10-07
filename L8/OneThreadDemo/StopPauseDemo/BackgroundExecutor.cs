using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StopPauseDemo
{
    public class BackgroundExecutor
    {
        public enum Status {
            STARTED, CANCELLED, FINISHED, SUSPENDED
        };

        public interface IListener {
            void OnStatusChanged(Status newStatus, string statusText);
        }

        private Thread _thread = null;
        private static bool _isCancellationRequested = false;
        private static IListener _listener = null;
        private ManualResetEvent _pauseEvent = new ManualResetEvent(true);

        public BackgroundExecutor(IListener listener) {
            _listener = listener;
        }

        public void Start() {
            _thread = new Thread(Solve);
            //_thread.IsBackground = true;
            //_thread.Priority = ThreadPriority.Normal;
            _thread.Start();
        }

        public void Cancel() {
            _isCancellationRequested = true;
        }

        public void Suspend()
        {
            //_thread.Suspend();  // deprecated method
            _pauseEvent.Reset();
            _listener?.OnStatusChanged(Status.SUSPENDED, "Paused...");
        }
        public void Resume()
        {
            //_thread.Resume();  // deprecated method
            _listener?.OnStatusChanged(Status.STARTED, "Resumed...");
            _pauseEvent.Set();
        }

        public bool IsSuspended()
        {
            return !_pauseEvent.WaitOne(0);
        }

        private void Solve()
        {
            _listener?.OnStatusChanged(Status.STARTED, "Thread started...");

            decimal res = 0;
            for (long i = 0; i < 20000000; i++)
            {
                if ((i & 0xFFFF) == 0)  // щоб не викликати WaitOne занадто часто, будемо викликати кожну 65536-ю ітерацію
                    _pauseEvent.WaitOne();

                if (_isCancellationRequested) {
                    _listener?.OnStatusChanged(Status.CANCELLED, "Cancelled!");
                    _isCancellationRequested = false;
                    return;
                }

                res += (decimal) Math.Sin(i);
            }

            _listener?.OnStatusChanged(Status.FINISHED, $"Thread Finished with res={res.ToString()}");
        }
    }
}
