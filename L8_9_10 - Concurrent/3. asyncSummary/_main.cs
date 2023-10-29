using System;

// Task - based Asynchronous Pattern(TAP), which uses a single method to represent the initiation
// and completion of an asynchronous operation. TAP was introduced in .NET Framework 4. It's the recommended
// approach to asynchronous programming in .NET. The async and await keywords in C# and the Async and Await
// operators in Visual Basic add language support for TAP. For more information, see Task-based Asynchronous
// Pattern (TAP).

namespace tapSample
{

    public class sampleApp
    {
        static void Main(string[] args)
        {
            //apmSample.Program.Main_();
            //eapSample.FibonacciForm.Main_();
            //tapSample.Program.Main_();
            raceConditionSample.Program.Main_();
            //threadPool.Program.Main_();
            Console.ReadLine();  // Pause
        }
    }
}

