// Демонстрація 3.2 — Start / Stop / Pause: ПОГАНИЙ дизайн (навмисно)
//
// Цей проект демонструє типові помилки керування потоками.
// Кращий підхід — у проекті StopPauseDemo (3.3).
//
// ═══════════════════════════════════════════════════════════════════
// ПРОБЛЕМА 1 — Thread.Abort() (закоментовано у btnStop)
//   Thread.Abort() кидає ThreadAbortException у довільній точці потоку.
//   Потік може бути всередині lock{}, using{}, або посередині запису у файл —
//   стан об'єктів залишиться пошкодженим, ресурси не звільняться.
//   У .NET 5+ метод взагалі кидає PlatformNotSupportedException.
//   → Правило: Abort() можна використовувати лише як "аварійне" завершення,
//     але не як нормальний сценарій зупинки.
//
// ПРОБЛЕМА 2 — Thread.Suspend() / Thread.Resume() (закоментовано)
//   Suspend() зупиняє потік у ДОВІЛЬНІЙ точці, навіть якщо він тримає lock.
//   Інші потоки (в тому числі UI), які чекають на той самий lock,
//   заблокуються назавжди → ДЕДЛОК.
//   Обидва методи deprecated і видалені з .NET 5+.
//   → Правильна альтернатива: ManualResetEvent (дивись StopPauseDemo).
//
// ПРОБЛЕМА 3 — bool _isCancellationRequested без volatile
//   Компілятор або процесор можуть кешувати значення bool у регістрі.
//   Фоновий потік не побачить зміни зробленої UI-потоком → нескінченний цикл.
//   Виправлення: оголосити поле як volatile bool, або використати
//   Interlocked.Exchange(), або CancellationTokenSource (рекомендовано).
//
// ПРОБЛЕМА 4 — кнопка Pause не робить паузу!
//   btnPause_Click лише змінює текст кнопки, але НЕ призупиняє потік.
//   Потік навіть не перевіряє стан паузи у своєму циклі.
//   → Правильна реалізація через ManualResetEvent — у StopPauseDemo.
// ═══════════════════════════════════════════════════════════════════

namespace StartStopDemo_2
{
    public partial class Form1 : Form
    {
        private Thread? _workerThread;

        // ⚠ ПРОБЛЕМА 3: поле без volatile — зміна з UI-потоку може бути
        // невидима фоновому потоку через кешування у процесорному регістрі.
        // Правильно: private volatile bool _isCancellationRequested = false;
        // Ще краще: CancellationTokenSource (дивись StopPauseDemo)
        private bool _isCancellationRequested = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            btnPause.Enabled = false;
            btnStart.Enabled = true;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = true;
            btnPause.Enabled = true;
            btnStart.Enabled = false;
            _isCancellationRequested = false;

            _workerThread = new Thread(() =>
            {
                Invoke(() => textBox1.Text = "Потік запущено...");

                decimal res = 0;
                for (long i = 0; i < 20_000_000; i++)
                {
                    // Перевірка прапора скасування — кооперативне завершення.
                    // Потік сам вирішує коли вийти, а не вбивається ззовні.
                    if (_isCancellationRequested)
                        break;

                    res += (decimal)Math.Sin(i);
                }

                Invoke(() =>
                {
                    textBox1.Text = !_isCancellationRequested
                        ? $"Завершено: res = {res}"
                        : "Скасовано!";

                    btnStop.Enabled = false;
                    btnPause.Enabled = false;
                    btnStart.Enabled = true;
                });
            });

            _workerThread.IsBackground = true;
            _workerThread.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            // ✔ Кооперативна зупинка: встановлюємо прапор, потік сам перевірить і вийде.
            _isCancellationRequested = true;

            // ❌ Thread.Abort() — НЕ ПРАВИЛЬНО:
            //    кидає ThreadAbortException у довільній точці,
            //    може залишити стан об'єктів пошкодженим.
            //    У .NET 5+ взагалі кидає PlatformNotSupportedException.
            // _workerThread?.Abort();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            // ⚠ ПРОБЛЕМА 4: ця кнопка НЕ ставить потік на паузу!
            // Вона лише змінює свій текст. Потік продовжує роботу.
            //
            // ❌ Thread.Suspend() — НЕ ПРАВИЛЬНО:
            //    зупиняє потік у ДОВІЛЬНІЙ точці, навіть якщо він тримає lock.
            //    Може спричинити дедлок. Видалено в .NET 5+.
            // _workerThread?.Suspend();
            //
            // ✔ Правильний підхід — ManualResetEvent.WaitOne() у тілі потоку.
            // Дивись StopPauseDemo (3.3).

            btnPause.Text = btnPause.Text == "Pause" ? "Resume" : "Pause";
            textBox1.Text = "⚠ Пауза не реалізована! Потік продовжує працювати.";
        }
    }
}