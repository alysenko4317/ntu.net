
using System;
using System.Threading;
using System.Threading.Tasks;

namespace tapSample
{
    class Program
    {
        // action delegate

        static void MyTask()   // public delegate void Action()
        {
            Console.WriteLine("MyTask() started");

            for (int count = 0; count < 10; count++)
            {
                Thread.Sleep(500);
                Console.WriteLine("MyTask:  count = " + count);
            }
        }

        // task creation
        public static void Main_0()
        {
            Console.WriteLine("Main thread started");

            Task task = new Task(MyTask);    // public Task(Action действие)
            task.Start();   // start task

            for (int i = 0; i < 60; i++)
            {
                Console.Write(".");
                Thread.Sleep(100);
            }

            Console.WriteLine("Main thread finished");
            Console.ReadLine();
        }

        // task creation with Task.Factory and lambda as a handler
        //    also Wait example
        //    also Dispose example

        public static void Main_1()
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

        // task continuation

        public static void Main_()
        {
            Task task1 = new Task(() => {
                Thread.Sleep(500);
                Console.WriteLine($"Current Task: {Task.CurrentId}");
            });

            // задача продолжения
            Task task2 = task1.ContinueWith(t => {
                Thread.Sleep(500);
                Console.WriteLine($"Current Task: {Task.CurrentId}  Previous Task: {t.Id}");
            });

            Task task3 = task2.ContinueWith(t => {
                Thread.Sleep(500);
                Console.WriteLine($"Current Task: {Task.CurrentId}  Previous Task: {t.Id}");
            });

            Task task4 = task3.ContinueWith(t => {
                Thread.Sleep(500);
                Console.WriteLine($"Current Task: {Task.CurrentId}  Previous Task: {t.Id}");
            });

            task1.Start();
            task4.Wait();   //  ждем завершения последней задачи

            Console.WriteLine("Конец метода Main");
        }
    }
}

