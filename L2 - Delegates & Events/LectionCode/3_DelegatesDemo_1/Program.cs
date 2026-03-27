using System;

class Program
{
    delegate void GetMessage();  // оголошення типу

    static void Main(string[] args)
    {
        GetMessage del;  // створення змінної делегатного типу

        if (DateTime.Now.Hour < 12)
            del = GoodMorning;
        else
            del = GoodEvening;

        del.Invoke();  // виклик делегату

        Console.ReadLine();
    }

    private static void GoodMorning() {
        Console.WriteLine("Good Morning");
    }

    private static void GoodEvening() {
        Console.WriteLine("Good Evening");
    }
}