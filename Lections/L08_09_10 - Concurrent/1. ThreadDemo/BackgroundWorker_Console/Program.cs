// Демонстрація 4.3 — BackgroundWorker у консольному застосунку
//
// Мета: показати що BackgroundWorker НЕ є WinForms-специфічним компонентом.
// Він знаходиться у System.ComponentModel і може використовуватись у будь-якому
// .NET-застосунку: Console, WPF, ASP.NET, бібліотеці тощо.
//
// Різниця від WinForms:
//   У WinForms BackgroundWorker автоматично маршалізує ProgressChanged і
//   RunWorkerCompleted у UI-потік через WindowsFormsSynchronizationContext.
//   У консольному застосунку SynchronizationContext.Current == null →
//   RunWorkerCompleted виконається просто у пулі потоків (ThreadPool),
//   а не в якомусь конкретному "головному" потоці.
//
// Що демонструється:
//   1. Запуск 10 воркерів паралельно, кожен з різною кількістю ітерацій
//   2. Кооперативне скасування через CancellationPending
//
// ── ПИТАННЯ ДО СТУДЕНТІВ (closure trap): ─────────────────────────────────────
//   У циклі:
//       for (int i = 0; i < 10; i++)
//       {
//           long j = i;    // ← навіщо ця локальна копія?
//           w.DoWork += (o, e) => { long iterationsCount = ITERATIONS[j]; ... }
//       }
//
//   Чому не можна написати ITERATIONS[i] напряму?
//   → Лямбда захоплює ЗМІННУ i, а не її ЗНАЧЕННЯ на момент створення.
//     До того часу як DoWork запуститься, цикл уже завершився і i == 10.
//     Всі 10 воркерів читатимуть ITERATIONS[10] → IndexOutOfRangeException!
//   → Виправлення: локальна копія  long j = i;  кожна лямбда захоплює свій j.
//   Те саме стосується будь-яких замикань у циклах (Thread, Task, LINQ тощо).

using System.ComponentModel;

namespace Application
{
    class Application
    {
        public static void Main()
        {
            long[] ITERATIONS = { 100_000, 1_000_000, 200_000, 3_000_000, 400_000,
                                  100_000, 1_000_000, 200_000, 3_000_000, 400_000 };

            List<BackgroundWorker> executors = new List<BackgroundWorker>();

            for (int i = 0; i < 10; i++)
            {
                BackgroundWorker w = new BackgroundWorker();
                w.WorkerSupportsCancellation = true;
                w.WorkerReportsProgress = false;

                // ✔ Локальна копія j — кожна лямбда захоплює своє власне j
                // ❌ Якби тут було ITERATIONS[i] — всі лямбди читали б i==10 після циклу
                long j = i;

                w.DoWork += (sender, e) =>
                {
                    Console.WriteLine($"Запущено воркер #{j}");

                    long iterationsCount = ITERATIONS[j];
                    BackgroundWorker worker = (BackgroundWorker)sender!;

                    decimal res = 0;
                    for (long k = 0; k < iterationsCount; k++)
                    {
                        if ((k & 0x3FFFF) == 0)
                        {
                            if (worker.CancellationPending)
                            {
                                e.Cancel = true;
                                return;
                            }
                        }
                        res += (decimal)Math.Sin(k);
                    }
                    e.Result = res;
                };

                w.RunWorkerCompleted += (sender, e) =>
                {
                    // У консолі немає UI-потоку → виконується в пулі потоків
                    if (e.Cancelled)
                        Console.WriteLine($"Воркер #{j}: скасовано");
                    else if (e.Error != null)
                        Console.WriteLine($"Воркер #{j}: помилка — {e.Error.Message}");
                    else
                        Console.WriteLine($"Воркер #{j}: результат = {e.Result}");
                };

                executors.Add(w);
            }

            // Запускаємо всі 10 воркерів — вони виконуються паралельно у ThreadPool
            foreach (BackgroundWorker w in executors)
            {
                Console.WriteLine("Запускаємо воркер...");
                w.RunWorkerAsync();
            }

            // Чекаємо завершення (Console.ReadLine утримує головний потік живим)
            Console.WriteLine("Всі воркери запущені. Натисніть Enter для виходу...");
            Console.ReadLine();
        }
    }
}
