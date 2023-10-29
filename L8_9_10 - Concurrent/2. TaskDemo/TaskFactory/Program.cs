using System;
using System.Threading;
using System.Threading.Tasks;

namespace task_2
{
    class Program
    {
        // --------------------------------------------------------------------------
        // task creation with Task.Factory and lambda as a handler
        //    also Wait example
        //    also Dispose example
        // --------------------------------------------------------------------------

        public static void Main()
        {
            Console.WriteLine("Основной поток запущен");

            Task task1 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("MyTask() №{0} запущен", Task.CurrentId);

                for (int count = 0; count < 10; count++)
                {
                    Thread.Sleep(500);
                    Console.WriteLine("В методе MyTask №{0} подсчет равен {1}", Task.CurrentId, count);
                }

                Console.WriteLine("MyTask() #{0} завершен", Task.CurrentId);
            });

            task1.Wait();
            task1.Dispose();

            Console.WriteLine("Основной поток завершен");
            Console.ReadLine();
        }
    }
}
