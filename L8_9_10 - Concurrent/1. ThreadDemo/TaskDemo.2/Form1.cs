namespace TaskDemo
{
    public partial class Form1 : Form
    {
        private Worker? _worker;

        public Form1()
        {
            InitializeComponent();
        }

        // --------------------------------------------
        // SAMPLE 1: Task class introduction
        // --------------------------------------------

        private void button1_Click(object sender, EventArgs e)
        {
            if (_worker != null) {
                MessageBox.Show("Worker is already running!");
                return;
            }

            _worker = new Worker();
            _worker.WorkCompleted += _worker_WorkCompleted;
            _worker.ProcessChanged += worker_ProcessChanged;

            button1.Enabled = false;

          //  Thread thread = new Thread(_worker.Work);
          //  thread.Start();

            var task = Task<bool>.Factory.StartNew(_worker.Work);

            bool cancelled = task.Result;   // тут буде OOPS!

            // Ситуація, коли контекст синхронізації (UI-потік) очікує виконання завдання,
            // а саме завдання чекає доступу до цього контексту, спричиняє дедлок. Це відбувається
            // через те, що асинхронний код намагається повернути результат у UI-потік, але він
            // заблокований очікуванням завершення завдання.
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_worker != null)
                _worker.Cancel();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Action action = () => {
                while (true) {
                    if (this.IsDisposed || label1.IsDisposed)
                        break;

                    Invoke((Action)(() => label1.Text = DateTime.Now.ToLongTimeString()));
                    Thread.Sleep(1000);
                }
            };

         //   Task task = new Task(action);
         //   task.Start();

            Task task = Task.Factory.StartNew(action);
        }

        private void _worker_WorkCompleted(bool cancelled)
        {
            this.InvokeEx(() =>
            {
                string message = cancelled ? "Процесс отменен" : "Процесс завершен!";
                MessageBox.Show(message);
                button1.Enabled = true;
            });
        }

        private void worker_ProcessChanged(int progress)
        {
            this.InvokeEx(() =>
            {
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

        public bool /*void(*/ Work()
        {
            for (int i = 0; i < 100; i++)
            {
                if (_cancelled)
                    break;

                Thread.Sleep(50);

                ProcessChanged(i);
            }

            WorkCompleted(_cancelled);

            return _cancelled;
        }

        public event Action<int> ProcessChanged;
        public event Action<bool> WorkCompleted;
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
