using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using MultiThreadTest;

namespace TaskDemo
{
    public partial class Form1 : Form
    {
        private WorkerRx _worker;
        private IDisposable _subscription;  // Збереження підписки для подальшого скасування

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Оновлення часу кожну секунду з використанням Reactive Extensions
            Observable.Interval(TimeSpan.FromSeconds(1))
                .ObserveOn(SynchronizationContext.Current)  // Планувальник для UI-потоку
                .Subscribe(x => label1.Text = DateTime.Now.ToLongTimeString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _worker = new WorkerRx();
            button1.Enabled = false;

            Action completed = () =>
            {
                string message = _worker.Cancelled ? "Процесс отменен" : "Процесс завершен!";
                MessageBox.Show(message);
                button1.Enabled = true;
            };

            Action<Exception> exception = ex =>
            {
                MessageBox.Show(ex.Message);
                button1.Enabled = true;
            };

            // Запускаємо роботу у фоновому потоці та відображаємо прогрес у UI
            _subscription = _worker.Work()
                .ToObservable()
                .SubscribeOn(TaskPoolScheduler.Default)  // Виконуємо на пулі потоків
                .ObserveOn(SynchronizationContext.Current)  // Оновлюємо інтерфейс на UI-потоці
                .Subscribe(x => progressBar.Value = x, exception, completed);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_worker != null)
                _worker.Cancel();  // Скасування роботи

            // Скасування підписки, якщо вона активна
            _subscription?.Dispose();
        }

        private void worker_ProcessChanged(int progress, int workId)
        {
            var progressBar = Controls.Find(string.Format("progressBar{0}", workId), false)
                .SingleOrDefault() as ProgressBar;

            if (progressBar != null)
            {
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
