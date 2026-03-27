using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    private static object _lock = new object();

    private static void TestLock()
    {
        int sum = 0;

        for (int i = 1; i <= 100; i++)
        {
            Thread thread = new Thread(() =>
            {
                lock (_lock)
                {
                    int x = sum;
                    Thread.Sleep(1);
                    x++;
                    sum = x;
                }
            });

            thread.Start();
        }

        Thread.Sleep(500);
        Console.WriteLine(sum);
    }

    private static void Main(string[] args)
    {
        TestLock();
        TestLock();
        TestLock();
        TestLock();
        TestLock();
        TestLock();
        TestLock();
        TestLock();

        Console.ReadLine();
    }
}
