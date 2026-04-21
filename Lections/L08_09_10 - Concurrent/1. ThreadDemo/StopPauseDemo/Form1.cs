using System.CodeDom.Compiler;
using System.Diagnostics;

namespace StopPauseDemo
{
    // Демонстрація 3.3 — Start / Stop / Pause: ПРАВИЛЬНИЙ дизайн
    //
    // ЩО ВИПРАВЛЕНО порівняно з StartStopDemo_2 (3.2):
    //
    //   ✔ Пауза реалізована через ManualResetEvent — кооперативна і безпечна
    //   ✔ Кооперативне скасування через volatile прапор — потік виходить сам
    //   ✔ BackgroundExecutor повністю відокремлений від UI через інтерфейс IListener
    //   ✔ Демонстрація відповідності UI (button1) — поки фон працює, форма жива
    //
    // ЯК ПРАЦЮЄ ManualResetEvent:
    //   ManualResetEvent(true)  — початковий стан: «відчинено» (потік не чекає)
    //   _pauseEvent.Reset()     — «зачиняємо» (наступний WaitOne() заблокує потік)
    //   _pauseEvent.Set()       — «відчиняємо» (потік продовжує виконання)
    //   _pauseEvent.WaitOne()   — потік чекає поки подія не буде Set()
    //   _pauseEvent.WaitOne(0)  — НЕ блокує, лише перевіряє стан (true = відчинено)
    //
    // ОПТИМІЗАЦІЯ РЕПОРТУ ПРОГРЕСУ:
    //   Виклик Invoke() (або WaitOne) на КОЖНІЙ ітерації — дуже дорого:
    //   кожен виклик це перемикання контексту між потоками.
    //   Тому перевірку потрібно виконувати рідше.
    //
    //   Варіант А — залишок від ділення (%)
    //     if (i % 100_000 == 0) ...
    //     Проблема: операція % — це цілочисельний поділ, одна з найдорожчих
    //     операцій процесора (десятки тактів).
    //
    //   Варіант Б — бітова маска & (РЕКОМЕНДОВАНО)
    //     if ((i & 0xFFFF) == 0) ...  // кожні 65 536 ітерацій
    //     if ((i & 0xFFFFF) == 0) ... // кожні 1 048 576 ітерацій
    //     Умова: N має бути степенем двійки мінус 1 (маска з одиниць).
    //     Операція & виконується за 1 такт процесора — у рази швидше %.
    //     Компромісно вибираємо 0xFFFF (~65K) або 0xFFFFF (~1M)
    //     залежно від того наскільки часто потрібно оновлювати прогрес.

    public partial class Form1 : Form, BackgroundExecutor.IListener
    {
        private readonly BackgroundExecutor _executor;

        public Form1()
        {
            InitializeComponent();
            _executor = new BackgroundExecutor(this);
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
            _executor.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            // Встановлюємо прапор скасування — потік сам перевіряє і виходить
            _executor.Cancel();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            // ManualResetEvent дозволяє коректно зупинити потік у контрольованій точці
            if (_executor.IsSuspended())
                _executor.Resume();
            else
                _executor.Suspend();
        }

        // ── Зворотний зв'язок від BackgroundExecutor через IListener ─────────
        // Викликається з фонового потоку → маршалізуємо через Invoke
        public void OnStatusChanged(BackgroundExecutor.Status newStatus, string statusText)
        {
            Invoke(() =>
            {
                textBox1.Text = statusText;

                switch (newStatus)
                {
                    case BackgroundExecutor.Status.SUSPENDED:
                        btnPause.Text = "▶  Resume";
                        break;

                    case BackgroundExecutor.Status.STARTED:
                    case BackgroundExecutor.Status.RESUMED:
                        btnPause.Text = "⏸  Pause";
                        break;

                    case BackgroundExecutor.Status.CANCELLED:
                    case BackgroundExecutor.Status.FINISHED:
                        btnStop.Enabled = false;
                        btnPause.Enabled = false;
                        btnStart.Enabled = true;
                        btnPause.Text = "⏸  Pause";
                        break;
                }
            });
        }

        // ── Демонстрація відповідальності UI ─────────────────────────────────
        // Поки фоновий потік виконується — форма залишається повністю живою.
        // Ця кнопка відкриває Калькулятор щоб наочно показати студентам,
        // що UI-потік вільний і реагує на дії користувача.
        private void btnOpenCalc_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("calc.exe");
        }
    }
}