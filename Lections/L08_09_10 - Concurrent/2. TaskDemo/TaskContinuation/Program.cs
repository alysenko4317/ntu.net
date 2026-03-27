using System;
using System.Threading;
using System.Threading.Tasks;

namespace taskContinuation
{
    class Program
    {
        // --------------------------------------------------------------------------
        // task continuation
        // --------------------------------------------------------------------------

        public static void Main()
        {
            Task task1 = new Task(() => {
                Thread.Sleep(500);
                Console.WriteLine($"task1, Current Task ID: {Task.CurrentId}");
            });

            // задача продолжения
            Task task2 = task1.ContinueWith(t =>
            {
                Thread.Sleep(500);
                Console.WriteLine($"task2, Current Task ID: {Task.CurrentId}  Previous Task: {t.Id}");
            }, TaskContinuationOptions.RunContinuationsAsynchronously);

            Task task3 = task2.ContinueWith(t => {
                Thread.Sleep(500);
                Console.WriteLine($"task3, Current Task ID: {Task.CurrentId}  Previous Task: {t.Id}");
            });

            Task task4 = task3.ContinueWith(t => {
                Thread.Sleep(500);
                Console.WriteLine($"task4, Current Task ID: {Task.CurrentId}  Previous Task: {t.Id}");
            });

            task1.Start();
            task4.Wait();   //  ждем завершения последней задачи

            Console.WriteLine("Конец метода Main");
        }
    }
}
