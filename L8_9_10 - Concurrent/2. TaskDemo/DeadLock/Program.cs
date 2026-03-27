using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static readonly object Lock1 = new object();
    static readonly object Lock2 = new object();

    static void Main()
    {
        Task task1 = Task.Run(() => DoWork1());
        Task task2 = Task.Run(() => DoWork2());

        Task.WaitAll(task1, task2);
    }

    static void DoWork1()
    {
        lock (Lock1)
        {
            Console.WriteLine("Task 1 acquired Lock1...");

            Thread.Sleep(100);  // Simulating some work
            lock (Lock2) {
                Console.WriteLine("Task 1 acquired Lock2...");
            }
        }
    }

    static void DoWork2()
    {
        lock (Lock2)
        {
            Console.WriteLine("Task 2 acquired Lock2...");

            Thread.Sleep(100);  // Simulating some work
            lock (Lock1) {
                Console.WriteLine("Task 2 acquired Lock1...");
            }
        }
    }
}

