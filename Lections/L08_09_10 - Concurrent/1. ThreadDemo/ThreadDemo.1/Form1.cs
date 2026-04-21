// Демонстрація 2.1 — Перший потік. Проблема блокування UI та повернення результату.
//
// ЗРАЗОК 1 — «Наївний» підхід: довга операція прямо в UI-потоці
//   Проблема: UI-потік зайнятий обчисленням → форма «замерзає».
//   Навіть рядок textBox1.Text = "Started" не відображається одразу,
//   бо цикл подій (message pump) не має змоги відпрацювати перемальовку.
//
// ЗРАЗОК 2 — Переносимо роботу у фоновий потік
//   ✔ UI залишається вільним
//   ✗ Проблема: як тепер повернути результат у форму?
//   BackgroundExecutor.Solve() — статичний метод без доступу до UI.
//   Пряме звернення form1.textBox1.Text = "..." кине InvalidOperationException
//   (cross-thread operation), а посилання на форму взагалі відсутнє.
//   → Рішення у наступному проекті (ThreadDemo.2).

namespace OneThreadDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // ── Зразок 1: довга операція в UI-потоці → UI замерзає ───────────────
        private void btnBlockingUI_Click(object sender, EventArgs e)
        {
            // Цей рядок буде «видно» лише після завершення циклу,
            // бо message pump не обробляється поки ми тут
            textBox1.Text = "Розпочато (але цей текст ви не побачите одразу!)";

            decimal res = 0;
            for (long i = 0; i < 100_000_000; i++)
                res += (decimal)Math.Sin(i);

            textBox1.Text = $"Результат: {res}  (UI був заблокований весь цей час)";
        }

        // ── Зразок 2: переносимо обчислення у фоновий потік ─────────────────
        // UI більше не блокується — але як отримати результат?
        private void btnBackgroundNoResult_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(BackgroundExecutor.Solve);
            // Thread.Start() — запускає метод у пулі (новому) потоці та ОДРАЗУ повертає керування
            t.IsBackground = true;
            t.Start();

            // Цей рядок виконається НЕГАЙНО після Start(), не чекаючи завершення потоку
            textBox1.Text = "Запущено у фоновому потоці... (але результат отримати не можемо!)";
        }

        // ── Виконавець (не знає про форму → не може оновити UI) ──────────────
        class BackgroundExecutor
        {
            public static void Solve()
            {
                decimal res = 0;
                for (long i = 0; i < 100_000_000; i++)
                    res += (decimal)Math.Sin(i);

                // ❌ Так НЕ можна — немає посилання на форму, і навіть якби було —
                //    звернення до UI з іншого потоку кине InvalidOperationException:
                // Application.OpenForms[0].???.Text = res.ToString();

                // → Потрібен механізм маршалінгу (Invoke/BeginInvoke або SynchronizationContext)
                // Дивись ThreadDemo.2
            }
        }
    }
}