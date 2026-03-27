using System;
using System.Threading;

namespace MultiThreadConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            for (int i = 1; i <= 10; i++)
            {
                //Thread thread = new Thread(Work);
                //thread.Start(i);

                ThreadPool.QueueUserWorkItem(Work, i);

                Thread.Sleep(200); // Затримка між стартами потоків
            }

            Console.ReadLine(); // Щоб консоль не закрилась одразу
        }

        private static void Work(object i)
        {
            Console.WriteLine("Ідентифікатор потоку: {0}, параметр: {1}",
                Thread.CurrentThread.ManagedThreadId, i);
        }
    }
}