namespace TaskDemo
{
    public partial class Form1 : Form
    {
        private Worker? _worker;
        private TaskScheduler _scheduler;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _scheduler = TaskScheduler.FromCurrentSynchronizationContext();

            Action action = () =>
            {
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
            if (_worker != null)
            {
                MessageBox.Show("Worker is already running!");
                return;
            }

            _worker = new Worker();
            _worker.ProcessChanged += worker_ProcessChanged;

            button1.Enabled = false;

            Task<bool> task = null;
            bool isError = false;
            bool cancelled = false;
            string message = "";

            try
            {
                task = Task<bool>.Factory.StartNew(() => _worker.Work());
                cancelled = await task;
            }
            catch (Exception ex)
            {
                isError = true;
                message = string.Format("Произошла ошибка: {0}", ex.Message);
            }

            if (!isError)
            {
                message = cancelled ? "Процесс отменен" : "Процесс завершен";
            }

            MessageBox.Show(message);
            button1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_worker != null)
                _worker.Cancel();
        }

        private void worker_ProcessChanged(int progress)
        {
            this.InvokeEx(() => {
                progressBar.Value = progress + (progress < 100 ? 1 : 0);
                progressBar.Value = progress + 1;
            });
        }
    }

    // ------------------------------------------------------------------
    // Worker
    // ------------------------------------------------------------------

    public class Worker
    {
        private bool _cancelled = false;

        public void Cancel()
        {
            _cancelled = true;
        }

        public bool Work()
        {
            for (int i = 0; i < 100; i++)
            {
                if (_cancelled)
                    break;

                if (i == 50)
                    throw new Exception("Щось пішло не так!");

                Thread.Sleep(50);

                ProcessChanged(i);
            }
            // TODO: release rsc
            return _cancelled;
        }

        public event Action<int> ProcessChanged;
    }

    // ------------------------------------------------------------------

    public static class ControlHelper
    {
        public static void InvokeEx(this Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }
    }
}
