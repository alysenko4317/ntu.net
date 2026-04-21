// Демонстрація 1.3 — Контекст синхронізації (SynchronizationContext)
//
// ЩО ВИПРАВЛЕНО / ПОКРАЩЕНО порівняно з проектом 1.2:
//   ✔ Воркер більше НЕ залежить від Control.Invoke() (WinForms-специфічний метод).
//   ✔ Маршалінг у UI-потік відбувається через абстракцію SynchronizationContext.
//   ✔ Клас Worker став повністю незалежним від UI-фреймворку.
//
// ПРОБЛЕМА проекту 1.2:
//   Control.Invoke() — це метод класу Control (WinForms).
//   Якщо перенести Worker у WPF — треба замінити на Dispatcher.Invoke().
//   Якщо в ASP.NET — там зовсім інший механізм.
//   Тобто Worker «знає» про конкретний UI-фреймворк → погана зв'язаність.
//
// РІШЕННЯ — SynchronizationContext:
//   Це абстрактний клас із BCL (System.Threading), який представляє
//   «контекст синхронізації» поточного середовища виконання.
//   Кожен UI-фреймворк надає свою реалізацію:
//
//       WinForms  →  WindowsFormsSynchronizationContext   (використовує Control.Invoke всередині)
//       WPF       →  DispatcherSynchronizationContext     (використовує Dispatcher.Invoke всередині)
//       ASP.NET   →  AspNetSynchronizationContext
//       Tests     →  можна підставити будь-який mock
//
//   Ключова ідея: UI-потік сам «публікує» свій контекст через
//   SynchronizationContext.Current, а Worker просто отримує його ззовні
//   (Dependency Injection через параметр або конструктор).
//   Таким чином Worker не залежить від WinForms/WPF/etc.
//
// Send() vs Post():
//   context.Send(callback, state) — синхронний виклик, чекає завершення (як Invoke)
//   context.Post(callback, state) — асинхронний, не чекає        (як BeginInvoke)

namespace Progress
{
    public partial class Form1 : Form
    {
        private Worker? _worker;

        // Зберігаємо контекст UI-потоку одразу при створенні форми.
        // SynchronizationContext.Current у конструкторі форми гарантовано
        // повертає WindowsFormsSynchronizationContext.
        private readonly SynchronizationContext _uiContext;

        public Form1()
        {
            InitializeComponent();

            // Capture UI synchronization context
            _uiContext = SynchronizationContext.Current
                         ?? throw new InvalidOperationException(
                             "SynchronizationContext.Current is null. " +
                             "Form must be created on a UI thread.");
        }

        // ── Обробник зміни прогресу ───────────────────────────────────────────
        // Цей метод викликається вже в UI-потоці (контекст про це подбав),
        // тому прямий доступ до progressBar є безпечним.
        private void OnProcessChanged(int progress)
        {
            progressBar.Value = progress;
            lblStatus.Text = $"Статус: виконується... {progress}%";
        }

        // ── Обробник завершення роботи ────────────────────────────────────────
        private void OnWorkCompleted(bool cancelled)
        {
            string message = cancelled ? "Процес скасовано" : "Процес завершено!";
            lblStatus.Text = $"Статус: {message}";
            MessageBox.Show(message);
            SetButtonsEnabled(true);
        }

        // ── Старт ─────────────────────────────────────────────────────────────
        private void btnStart_Click(object sender, EventArgs e)
        {
            SetButtonsEnabled(false);
            lblStatus.Text = "Статус: запущено...";

            // Передаємо контекст у воркер — Dependency Injection «по-простому».
            // Worker не знає, що це WinForms: він отримує лише абстракцію.
            _worker = new Worker(_uiContext);
            _worker.ProcessChanged += OnProcessChanged;
            _worker.WorkCompleted  += OnWorkCompleted;

            Thread workerThread = new Thread(_worker.Work);
            workerThread.IsBackground = true;
            workerThread.Start();
        }

        // ── Stop ──────────────────────────────────────────────────────────────
        private void btnStop_Click(object sender, EventArgs e)
        {
            _worker?.Cancel();
        }

        private void SetButtonsEnabled(bool enabled)
        {
            btnStart.Enabled = enabled;
        }
    }

    // ── Робочий клас — НЕ знає нічого про WinForms / WPF / etc. ─────────────
    //
    // Worker отримує SynchronizationContext через конструктор.
    // Він може працювати в будь-якому UI-фреймворку без змін.
    public class Worker
    {
        private readonly SynchronizationContext _context;
        private bool _cancelled = false;

        public event Action<int>?  ProcessChanged;
        public event Action<bool>? WorkCompleted;

        // Контекст передається ззовні — Worker не прив'язаний до конкретного фреймворку
        public Worker(SynchronizationContext context)
        {
            _context = context;
        }

        public void Cancel() => _cancelled = true;

        public void Work()
        {
            for (int i = 1; i <= 100; i++)
            {
                if (_cancelled) break;

                Thread.Sleep(50);

                // context.Send() — синхронно маршалізує виклик у UI-потік.
                // Під капотом WinForms: Control.Invoke()
                // Під капотом WPF:     Dispatcher.Invoke()
                _context.Send(_ => ProcessChanged?.Invoke(i), null);
            }

            // Також безпечно — виконається у UI-потоці
            _context.Send(_ => WorkCompleted?.Invoke(_cancelled), null);
        }
    }
}
