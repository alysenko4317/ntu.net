// Демонстрація 4.1 — BackgroundWorker: базова демонстрація (з навмисними недоліками)
//
// BackgroundWorker — компонент із System.ComponentModel, що інкапсулює:
//   ✔ Запуск роботи у фоновому потоці (ThreadPool)
//   ✔ Маршалінг подій ProgressChanged та RunWorkerCompleted у UI-потік
//     автоматично через SynchronizationContext — БЕЗ жодного Invoke()!
//   ✔ Механізм скасування (CancelAsync / CancellationPending)
//   ✔ Передачу результату та обробку помилок через RunWorkerCompleted
//
// Три ключові події:
//   DoWork             — виконується у фоновому потоці (ThreadPool)
//                        ⚠ НЕ звертатись до UI-елементів напряму!
//   ProgressChanged    — маршалізується у UI-потік автоматично
//                        (потрібно WorkerReportsProgress = true)
//   RunWorkerCompleted — маршалізується у UI-потік автоматично
//                        Тут обробляємо результат, помилку або скасування
//
// ═══════════════════════════════════════════════════════════════════
// НАВМИСНІ НЕДОЛІКИ цього прикладу (для обговорення зі студентами):
//
// НЕДОЛІК 1 — Unreachable code у DoWork:
//   throw new Exception("щось пішло не так!");  ← виконується першим
//   e.Cancel = true;                             ← НІКОЛИ не виконається!
//   break;                                       ← НІКОЛИ не виконається!
//   Правильно: перевірити CancellationPending → встановити e.Cancel = true → return
//   Дивись виправлену версію у BackgroundWorker.2
//
// НЕДОЛІК 2 — «Костиль» з ProgressBar:
//   progressBar1.Value = e.ProgressPercentage + 1;  // спочатку +1...
//   progressBar1.Value = e.ProgressPercentage;       // ...потім правильне значення
//   Причина: у WinForms ProgressBar анімує рух уперед плавно,
//   але НЕ анімує рух назад — миттєво відображає менше значення.
//   Хак «+1 потім -1» змушує анімацію «дострибнути» до потрібної позиції.
//   Це відомий workaround для WinForms ProgressBar, але виглядає жахливо.
// ═══════════════════════════════════════════════════════════════════

namespace BackgroundWorker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // ── Start ─────────────────────────────────────────────────────────────
        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            textBox1.Text = "Запущено...";
            progressBar1.Show();

            // RunWorkerAsync() — запускає DoWork у фоновому потоці (ThreadPool)
            // і повертається ОДРАЗУ. UI залишається вільним.
            backgroundWorker1.RunWorkerAsync();
        }

        // ── DoWork — виконується у фоновому потоці ────────────────────────────
        // ⚠ НЕ звертатись тут до progressBar1, textBox1 тощо напряму!
        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            const long iterationsCount = 20_000_000;
            decimal res = 0;

            for (long i = 0; i < iterationsCount; i++)
            {
                if ((i & 0x3FFFF) == 0)  // кожні 262 144 ітерацій (2^18)
                {
                    int progress = (int)((i + 1.0) / iterationsCount * 100);
                    // ReportProgress() → маршалізує ProgressChanged у UI-потік
                    backgroundWorker1.ReportProgress(progress);

                    if (backgroundWorker1.CancellationPending)
                    {
                        // ⚠ НЕДОЛІК 1: throw виконується ДО e.Cancel = true!
                        // Все що нижче — unreachable code (ніколи не виконається).
                        // Виняток потрапить у RunWorkerCompleted як e.Error, а не e.Cancelled.
                        throw new Exception("Скасування через виняток — НЕПРАВИЛЬНО!");
                        e.Cancel = true;   // ← НЕДОСЯЖНИЙ КОД
                        break;             // ← НЕДОСЯЖНИЙ КОД
                    }
                }
                res += (decimal)Math.Sin(i);
            }
            // Результат передаємо через e.Result → доступний у RunWorkerCompleted
            e.Result = res;
        }

        // ── ProgressChanged — маршалізується у UI-потік автоматично ──────────
        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            textBox1.Text = $"Виконується... {e.ProgressPercentage}%";

            // ⚠ НЕДОЛІК 2: «Костиль» для анімації WinForms ProgressBar.
            // ProgressBar анімує рух уперед, але миттєво стрибає назад.
            // Хак: спочатку встановити +1 (анімація доходить до цілі), потім правильне.
            progressBar1.Value = Math.Min(e.ProgressPercentage + 1, progressBar1.Maximum);
            progressBar1.Value = e.ProgressPercentage;
        }

        // ── RunWorkerCompleted — маршалізується у UI-потік автоматично ────────
        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            progressBar1.Hide();
            btnStart.Enabled = true;
            btnStop.Enabled = false;

            // e.Error — якщо DoWork кинув виняток (у т.ч. наш "навмисний")
            if (e.Error != null)
            {
                textBox1.Text = $"Помилка: {e.Error.Message}";
                return;
            }
            // e.Cancelled — якщо e.Cancel = true у DoWork (але через throw — не спрацює!)
            if (e.Cancelled)
            {
                textBox1.Text = "Скасовано!";
                return;
            }
            textBox1.Text = $"Завершено! Результат = {e.Result}";
        }

        // ── Stop ──────────────────────────────────────────────────────────────
        private void btnStop_Click(object sender, EventArgs e)
        {
            // CancelAsync() лише встановлює прапор CancellationPending = true.
            // Сам потік НЕ зупиняється — він має перевірити прапор у DoWork.
            backgroundWorker1.CancelAsync();
            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }
    }
}