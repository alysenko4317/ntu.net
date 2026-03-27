
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace tapSample
{
    class Program
    {
        // --------------------------------------------------------------------------
        // task parallel execution demonstration
        // --------------------------------------------------------------------------

        static void MyTask()   // action delegate:  public delegate void Action()
        {
            Console.WriteLine("MyTask() started");

            for (int count = 0; count < 10; count++)
            {
                Thread.Sleep(500);
                Console.WriteLine("MyTask:  count = " + count);
            }
        }

        public static void Main()
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
    }
}
