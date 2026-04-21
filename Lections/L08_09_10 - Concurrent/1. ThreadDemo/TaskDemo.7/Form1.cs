using MultiThreadTest;

namespace TaskDemo
{
    public partial class Form1 : Form
    {
        private TaskScheduler _scheduler;
        private CancellationTokenSource _tokenSource;

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _scheduler = TaskScheduler.FromCurrentSynchronizationContext();

            Action action = () => {
                while (true)
                {
                    if (this.IsDisposed || label1.IsDisposed)
                        break;

                    Invoke((Action)(() => label1.Text = DateTime.Now.ToLongTimeString()));

                    Thread.Sleep(1000);
                }
            };

            Task task = Task.Factory.StartNew(action);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            _tokenSource = new CancellationTokenSource();
            CancellationToken token = _tokenSource.Token;

            button1.Enabled = false;

            List<Task<int>> tasks = new List<Task<int>>();

            for (int i = 1; i <= 5; i++)
            {
                var worker = new WorkerMultiTask(i);

                worker.ProcessChanged += worker_ProcessChanged;

                Task<int> task = Task<int>.Factory.StartNew(worker.Work, new { Delay = i * 10, Token = token });

                tasks.Add(task);
            }

            try
            {
                int result = await Task<int>.Factory.ContinueWhenAll(tasks.ToArray(), ts => ts.Sum(t => t.Result));

                MessageBox.Show(string.Format("Процесс завершен. Результат: {0}", result));
            }
            catch (AggregateException ex)
            {
                ex.Flatten().Handle(exc =>
                {
                    string message = exc is TaskCanceledException ? "Процесс отменен." : exc.Message;

                    MessageBox.Show(message);
                    return true;  // це говорить платформі що ми самі обробили виключення
                });
            }

            button1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_tokenSource != null) {
                _tokenSource.Cancel();
            }
        }

        private void worker_ProcessChanged(int progress, int workId)
        {
            var progressBar = Controls.Find(string.Format("progressBar{0}", workId), false)
                .SingleOrDefault() as ProgressBar;

            if (progressBar != null) {
                this.InvokeEx(() => {
                    progressBar.Value = progress + 1;
                    progressBar.Value = progress;
                });
            }
        }
    }

    // ------------------------------------------------------------------

    public static class ControlHelper
    {
        public static void InvokeEx(this Control control, Action action)
        {
            if (control.InvokeRequired) {
                control.Invoke(action);
            }
            else {
                action();
            }
        }
    }
}
