namespace Progress
{
    public partial class Form1 : Form
    {
        private Worker _worker;
        private readonly SynchronizationContext? _context;

        public Form1()
        {
            InitializeComponent();
            _context = SynchronizationContext.Current;
        }

        private void _worker_WorkCompleted(bool cancelled)
        {
            string message = cancelled ? "Процесс отменен" : "Процесс завершен!";
            MessageBox.Show(message);
            btnStart.Enabled = true;  // Включити кнопку старту
        }

        private void _worker_ProcessChanged(int progress)
        {
            // Оновити інтерфейс для відображення прогресу (наприклад, ProgressBar)
            progressBar.Value = progress; //(як приклад)
        }

        private void btnStart_MouseClick(object sender, MouseEventArgs e)
        {
            _worker = new Worker();
            _worker.WorkCompleted += _worker_WorkCompleted;
            _worker.ProcessChanged += _worker_ProcessChanged;

            //_worker.Work();  // UI will be blocked here

            // Почати роботу в окремому потоці
            Thread workerThread = new Thread(_worker.Work);
            workerThread.Start(_context);

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

        public void Work(object param)
        {
            SynchronizationContext context = (SynchronizationContext)param;

            for (int i = 0; i <= 99; i++)
            {
                if (_cancelled)
                    break;

                Thread.Sleep(50);

                context.Send(OnProgressChanged, i);
            }

            context.Send(OnWorkCompleted, _cancelled);
        }

        public void OnProgressChanged(object i)
        {
            if (ProcessChanged != null)
                ProcessChanged((int)i);
        }

        public void OnWorkCompleted(object cancelled)
        {
            if (WorkCompleted != null)
                WorkCompleted((bool)cancelled);
        }

        public event Action<int> ProcessChanged;
        public event Action<bool> WorkCompleted;
    }
}
