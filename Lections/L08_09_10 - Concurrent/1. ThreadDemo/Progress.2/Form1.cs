namespace Progress
{
    public partial class Form1 : Form
    {
        private Worker _worker;

        public Form1()
        {
            InitializeComponent();
        }

        private void _worker_WorkCompleted(bool cancelled)
        {
            Action action = () =>
            {
                string message = cancelled ? "Процес відмінено" : "Процес завершено!";
                MessageBox.Show(message);
                btnStart.Enabled = true;
            };

            // Invoke(action);
            this.InvokeEx(action);  // optimization
        }

        private void _worker_ProcessChanged(int progress)
        {
            Action action = () => {
                // Оновити інтерфейс для відображення прогресу (наприклад, ProgressBar)
                progressBar.Value = progress;
            };

            //Invoke(action);
            this.InvokeEx(action);  // optimization
        }

        private void btnStart_MouseClick(object sender, MouseEventArgs e)
        {
            _worker = new Worker();
            _worker.WorkCompleted += _worker_WorkCompleted;
            _worker.ProcessChanged += _worker_ProcessChanged;

            //_worker.Work();  // UI will be blocked here

            // Почати роботу в окремому потоці
            Thread workerThread = new Thread(_worker.Work);
            workerThread.Start();

            btnStart.Enabled = false;  // Вимкнути кнопку старту
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _worker?.Cancel();  // Викликати метод Cancel для скасування
        }
    }

    // ------------------------------------------------------------------

    public class Worker
    {
        private bool _cancelled = false;

        public void Cancel()
        {
            _cancelled = true;
        }

        public void Work()
        {
            for (int i = 1; i <= 100; i++)
            {
                if (_cancelled)
                    break;

                Thread.Sleep(50);

                ProcessChanged(i);
            }

            WorkCompleted(_cancelled);
        }

        public event Action<int> ProcessChanged;
        public event Action<bool> WorkCompleted;
    }

    // ------------------------------------------------------------------

    public static class ControlHelper {
        public static void InvokeEx(this Control control, Action action) {
            if (control.InvokeRequired) {
                control.Invoke(action);
            }
            else {
                action();
            }
        }
    }
}
