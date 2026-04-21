using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StopPauseDemo
{
    // ── BackgroundExecutor — ізольований від UI виконавець ───────────────────
    //
    // Взаємодія з UI відбувається виключно через інтерфейс IListener.
    // Завдяки цьому BackgroundExecutor не має жодної залежності від WinForms:
    // він не знає про Control, Invoke, Form — лише про IListener.
    //
    // ── Механізм паузи: ManualResetEvent ─────────────────────────────────────
    //   ManualResetEvent — це примітив синхронізації з двома станами:
    //     Set   (signaled)   — «двері відчинені»: WaitOne() повертається одразу
    //     Reset (unsignaled) — «двері зачинені»:  WaitOne() блокує потік
    //
    //   Початковий стан new ManualResetEvent(true) → «відчинено», потік не чекає.
    //   Suspend(): _pauseEvent.Reset() → зачиняємо, наступний WaitOne() заблокує.
    //   Resume():  _pauseEvent.Set()   → відчиняємо, потік продовжується.
    //
    // ── Оптимізація частоти перевірок ────────────────────────────────────────
    //   Викликати WaitOne() або Invoke() на кожній ітерації дуже дорого:
    //   кожен такий виклик — це системний виклик або перемикання контексту.
    //
    //   Погано (кожна ітерація):
    //     _pauseEvent.WaitOne();  // 200 000 000 системних викликів!
    //
    //   Краще, але ще є недолік (залишок від ділення):
    //     if (i % 100_000 == 0) _pauseEvent.WaitOne();
    //     Операція % — цілочисельний поділ: ~20-30 тактів процесора.
    //
    //   Найкраще (бітова маска):
    //     if ((i & 0xFFFF) == 0) _pauseEvent.WaitOne();
    //     0xFFFF  = 65 535  → перевірка кожні 65 536 ітерацій  (2^16)
    //     0xFFFFF = 1 048 575 → перевірка кожні 1 048 576 ітерацій (2^20)
    //     Операція & — бітове «і»: 1 такт процесора.
    //     Ця оптимізація працює лише якщо N є степенем двійки.

    public class BackgroundExecutor
    {
        public enum Status
        {
            STARTED, RESUMED, SUSPENDED, CANCELLED, FINISHED
        }

        public interface IListener
        {
            void OnStatusChanged(Status newStatus, string statusText);
        }

        private Thread? _thread;

        // ✔ volatile — гарантує що зміна з UI-потоку буде видна фоновому потоку
        //   без кешування у процесорному регістрі
        private volatile bool _isCancellationRequested = false;

        // ⚠ BUG у попередній версії: ці поля були static!
        //   static означає одне значення на весь клас — якщо створити два екземпляри,
        //   вони ділитимуть один _listener і один прапор скасування → хаос.
        //   Виправлено: instance-поля (без static)
        private readonly IListener _listener;

        // ManualResetEvent(true) — початковий стан «відчинено» (виконання не заблоковано)
        private readonly ManualResetEvent _pauseEvent = new ManualResetEvent(true);

        public BackgroundExecutor(IListener listener)
        {
            _listener = listener;
        }

        public void Start()
        {
            _isCancellationRequested = false;
            _pauseEvent.Set(); // На випадок якщо попередній сеанс завершився на паузі

            _thread = new Thread(Solve);
            _thread.IsBackground = true;
            _thread.Priority = ThreadPriority.Normal;
            _thread.Start();
        }

        // Кооперативне скасування: встановлюємо прапор, потік сам перевіряє і виходить.
        // Якщо потік на паузі — розблоковуємо, щоб він міг перевірити прапор і вийти.
        public void Cancel()
        {
            _isCancellationRequested = true;
            _pauseEvent.Set(); // розблокуємо якщо потік чекав на паузі
        }

        public void Suspend()
        {
            _pauseEvent.Reset(); // зачиняємо «двері» — потік зупиниться на наступному WaitOne()
            _listener.OnStatusChanged(Status.SUSPENDED, "Призупинено...");
        }

        public void Resume()
        {
            _listener.OnStatusChanged(Status.RESUMED, "Відновлено...");
            _pauseEvent.Set(); // відчиняємо «двері» — потік продовжує виконання
        }

        // WaitOne(0) — не блокує, лише перевіряє стан: false = зачинено = потік на паузі
        public bool IsSuspended() => !_pauseEvent.WaitOne(0);

        private void Solve()
        {
            _listener.OnStatusChanged(Status.STARTED, "Потік запущено...");

            decimal res = 0;
            for (long i = 0; i < 200_000_000; i++)
            {
                // Оптимізована перевірка паузи та скасування:
                // використовуємо бітову маску замість % — операція & виконується за 1 такт.
                // 0xFFFF = 65535 → перевірка кожні 65 536 ітерацій.
                //
                // Порівняння підходів:
                //   if (i % 65_536 == 0)    ← правильно, але % дорогий (~25 тактів)
                //   if ((i & 0xFFFF) == 0)  ← те саме, але & за 1 такт ✔
                if ((i & 0xFFFF) == 0)
                {
                    // WaitOne() — чекаємо якщо потік на паузі (ManualResetEvent.Reset())
                    // Якщо не на паузі — повертається одразу без затримки
                    _pauseEvent.WaitOne();

                    if (_isCancellationRequested)
                    {
                        _listener.OnStatusChanged(Status.CANCELLED, "Скасовано!");
                        return;
                    }
                }

                res += (decimal)Math.Sin(i);
            }

            _listener.OnStatusChanged(Status.FINISHED, $"Завершено: res = {res}");
        }
    }
}
