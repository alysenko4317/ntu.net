// Демонстрація 1.1 — Проблема відповідальності UI-потоку
//
// ПРОБЛЕМА 1: Якщо довга операція виконується прямо в UI-потоці —
//             інтерфейс "замерзає" і не реагує на дії користувача.
//
// ПРОБЛЕМА 2: Якщо операцію перенести в окремий потік —
//             спроба оновити UI-елемент з цього потоку кидає
//             InvalidOperationException (Cross-thread operation not valid).
//
// ВИСНОВОК:   Для оновлення UI з фонового потоку необхідно використовувати
//             Control.Invoke() / Control.BeginInvoke()  (див. наступний проект).

namespace Progress
{
    public partial class Form1 : Form
    {
        private Worker? _worker;

        public Form1()
        {
            InitializeComponent();
        }

        // ── Сценарій 1: виклик роботи ПРЯМО в UI-потоці ─────────────────────
        // UI повністю замерзає до завершення циклу.
        // Кнопка "Stop" не реагує, вікно не перемальовується.
        private void btnStartBlocking_Click(object sender, EventArgs e)
        {
            SetButtonsEnabled(false);
            lblStatus.Text = "Статус: працює (UI заблокований)...";

            _worker = new Worker();
            _worker.ProcessChanged += progress => progressBar.Value = progress;
            _worker.WorkCompleted += OnWorkCompleted;

            _worker.Work(); // ← UI-потік виконує цикл → інтерфейс заморожено
        }

        // ── Сценарій 2: робота в окремому потоці — БЕЗ Invoke ────────────────
        // UI НЕ замерзає, але при першій же спробі змінити progressBar
        // з фонового потоку виникає InvalidOperationException:
        //   "Cross-thread operation not valid: Control accessed from a thread
        //    other than the thread it was created on."
        private void btnStartThread_Click(object sender, EventArgs e)
        {
            SetButtonsEnabled(false);
            lblStatus.Text = "Статус: запущено в окремому потоці (очікуємо виняток)...";

            _worker = new Worker();
            _worker.ProcessChanged += progress => progressBar.Value = progress; // ← ВИНЯТОК тут
            _worker.WorkCompleted += OnWorkCompleted;

            Thread workerThread = new Thread(_worker.Work);
            workerThread.IsBackground = true;
            workerThread.Start(); // ← запустили фоновий потік
            // UI вільний, але оновлення елементів керування кине виняток!
        }

        // ── Stop ──────────────────────────────────────────────────────────────
        private void btnStop_Click(object sender, EventArgs e)
        {
            _worker?.Cancel();
        }

        // ── Спільний обробник завершення ──────────────────────────────────────
        private void OnWorkCompleted(bool cancelled)
        {
            // Увага: цей метод може викликатися з фонового потоку (сценарій 2),
            // тому звертання до UI тут також може кинути виняток!
            string message = cancelled ? "Процес скасовано" : "Процес завершено!";
            lblStatus.Text = $"Статус: {message}";
            MessageBox.Show(message);
            SetButtonsEnabled(true);
        }

        private void SetButtonsEnabled(bool enabled)
        {
            btnStartBlocking.Enabled = enabled;
            btnStartThread.Enabled = enabled;
        }
    }

    // ── Робочий клас (не знає нічого про UI) ─────────────────────────────────
    public class Worker
    {
        private bool _cancelled = false;

        public event Action<int>?  ProcessChanged;
        public event Action<bool>? WorkCompleted;

        public void Cancel() => _cancelled = true;

        public void Work()
        {
            for (int i = 1; i <= 100; i++)
            {
                if (_cancelled) break;
                Thread.Sleep(50);
                ProcessChanged?.Invoke(i);   // ← виклик з потоку, де виконується Work()
            }
            WorkCompleted?.Invoke(_cancelled);
        }
    }
}
