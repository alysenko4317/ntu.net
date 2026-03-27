
using System;
using System.Xml.Serialization;

class Program
{
    static void Main(string[] args)
    {
        Action action = null;

        for (int cycleCounter = 1; cycleCounter <= 4; cycleCounter++)
        {
            action += () => Console.WriteLine(cycleCounter);
            
        }

        action();

        Console.ReadLine();
    }
}