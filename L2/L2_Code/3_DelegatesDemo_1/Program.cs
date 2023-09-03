using System;

class Program
{
    delegate void GetMessage();

    static void Main(string[] args)
    {
        GetMessage del;

        if (DateTime.Now.Hour < 12)
            del = GoodMorning;
        else
            del = GoodEvening;

        del.Invoke();
        Console.ReadLine();
    }

    private static void GoodMorning() {
        Console.WriteLine("Good Morning");
    }

    private static void GoodEvening() {
        Console.WriteLine("Good Evening");
    }
}