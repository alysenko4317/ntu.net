// Демонстрація 1.2 — Введення у потоки. Рішення: Control.Invoke()
//
// ЩО ВИПРАВЛЕНО порівняно з проектом 1.1:
//   ✔ Робота запускається в окремому потоці — UI НЕ замерзає.
//   ✔ Оновлення UI виконується через Invoke() — немає InvalidOperationException.
//
// ЯК ПРАЦЮЄ Invoke():
//   Метод Control.Invoke(action) ставить делегат у чергу UI-потоку і чекає
//   його виконання. Тобто action() гарантовано виконається у UI-потоці,
//   навіть якщо Invoke() викликано з фонового потоку.
//
// ОПТИМІЗАЦІЯ — InvokeRequired:
//   Якщо Invoke() викликати з UI-потоку (наприклад, у сценарії блокування),
//   це зайвий «стрибок» через чергу — марна трата ресурсів.
//   Властивість InvokeRequired повертає true лише тоді, коли ми справді
//   знаходимося в іншому потоці. Завдяки цьому метод-розширення InvokeEx()
//   працює ефективно в обох випадках:
//
//       if (InvokeRequired)
//           Invoke(action);   // викликаємо через чергу UI-потоку
//       else
//           action();         // вже в UI-потоці — виконуємо напряму

namespace Progress
{
    public partial class Form1 : Form
    {
        private Worker? _worker;

        public Form1()
        {
            InitializeComponent();
        }

        // ── Обробник зміни прогресу (викликається з фонового потоку) ─────────
        // Без Invoke тут буде InvalidOperationException (як у проекті 1.1).
        // InvokeEx() перевіряє InvokeRequired і маршалізує виклик у UI-потік.
        private void OnProcessChanged(int progress)
        {
            // Варіант 1 (наївний, без оптимізації):
            //   Invoke(() => progressBar.Value = progress);

            // Варіант 2 (з оптимізацією через InvokeRequired):
            this.InvokeEx(() => progressBar.Value = progress);
        }

        // ── Обробник завершення роботи (також з фонового потоку) ─────────────
        private void OnWorkCompleted(bool cancelled)
        {
            this.InvokeEx(() =>
            {
                string message = cancelled ? "Процес скасовано" : "Процес завершено!";
                lblStatus.Text = $"Статус: {message}";
                MessageBox.Show(message);
                SetButtonsEnabled(true);
            });
        }

        // ── Старт ─────────────────────────────────────────────────────────────
        private void btnStart_Click(object sender, EventArgs e)
        {
            SetButtonsEnabled(false);
            lblStatus.Text = "Статус: виконується у фоновому потоці...";

            _worker = new Worker();
            _worker.ProcessChanged += OnProcessChanged;
            _worker.WorkCompleted  += OnWorkCompleted;

            // Запускаємо роботу в окремому потоці — UI залишається вільним
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
                ProcessChanged?.Invoke(i);
            }
            WorkCompleted?.Invoke(_cancelled);
        }
    }

    // ── Допоміжний метод-розширення з оптимізацією InvokeRequired ────────────
    //
    // Замість того, щоб завжди писати:
    //     if (control.InvokeRequired)
    //         control.Invoke(action);
    //     else
    //         action();
    //
    // Використовуємо один зручний виклик:
    //     control.InvokeEx(action);
    public static class ControlHelper
    {
        public static void InvokeEx(this Control control, Action action)
        {
            if (control.InvokeRequired)
                control.Invoke(action);  // маршалізуємо у UI-потік
            else
                action();                // вже в UI-потоці — напряму
        }
    }
}
