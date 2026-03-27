using System;
using System.Threading;

public class TestLock
{
    // Shared lock object for synchronization
    private readonly object _key = new object();

    public void TestLockWithLock()
    {
        object key = new object();

        // Enter critical section
       // lock ("some_string")
        lock (_key)
        {
            int id = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine("Thread {0} started working", id);
            Thread.Sleep(2000); // Simulate work
            Console.WriteLine("Thread {0} finished working", id);
        }
    }
}

internal class Program
{
    private static void Main(string[] args)
    {
        TestLock test = new TestLock();

        // Create two threads that execute the same method
        Thread thread1 = new Thread(test.TestLockWithLock);
        Thread thread2 = new Thread(test.TestLockWithLock);

        thread1.Start();
        thread2.Start();

        // Wait for both threads to complete
        thread1.Join();
        thread2.Join();

        Console.WriteLine("All threads have completed.");
        Console.ReadLine();
    }
}