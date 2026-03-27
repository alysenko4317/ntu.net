
using System;
using System.Threading;

// Asynchronous Programming Model(APM) pattern(also called the IAsyncResult pattern),
// which is the legacy model that uses the IAsyncResult interface to provide asynchronous
// behavior. In this pattern, synchronous operations require Begin and End methods (for example,
// BeginWrite and EndWrite to implement an asynchronous write operation). This pattern is no longer
// recommended for new development.For more information, see Asynchronous Programming Model(APM).

namespace apmSample
{
    public class AsyncDemo
    {
        // The method to be executed asynchronously.
        public string TestMethod(int callDuration, out int threadId)
        {
            Console.WriteLine("Test method begins.");

            Thread.Sleep(callDuration);
            threadId = Thread.CurrentThread.ManagedThreadId;

            return String.Format("My call time was {0}.", callDuration.ToString());
        }
    }

    // The delegate must have the same signature as the method it will call asynchronously.
    public delegate string AsyncMethodCaller(int callDuration, out int threadId);

    class Program
    {
        // Starting with the.NET Framework 4, the Task Parallel Library provides a new model
        // for asynchronous and parallel programming.For more information, see Task Parallel Library (TPL)
        // and Task-based Asynchronous Pattern (TAP)).

        public static void Main_()
        {
            // The asynchronous method puts the thread id here.
            int threadId;

            // Create an instance of the test class.
            AsyncDemo ad = new AsyncDemo();

            // Create the delegate.
            AsyncMethodCaller caller = new AsyncMethodCaller(ad.TestMethod);

            // Initiate the asychronous call.
            IAsyncResult result = caller.BeginInvoke(3000, out threadId, null, null);

            //Thread.Sleep(0);
            Console.WriteLine(
                "Main thread {0} does some work.", Thread.CurrentThread.ManagedThreadId);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            // Perform additional processing here.
            // Call EndInvoke to retrieve the results.
            string returnValue = caller.EndInvoke(out threadId, result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            Console.WriteLine("The call executed on thread {0}, with return value \"{1}\".",
                threadId, returnValue);
        }
    }
}
