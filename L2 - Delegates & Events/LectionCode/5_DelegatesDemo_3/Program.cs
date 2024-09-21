using System;

class Program
{
    delegate void GetMessage();

    static void Main(string[] args)
    {
        if (DateTime.Now.Hour < 12)
            ShowMessage(GoodMorning);
        else
            ShowMessage(GoodEvening);

        Console.ReadLine();
    }

    // передача делегату як параметра методу
    private static void ShowMessage(GetMessage _del) {
        //_del.Invoke();
        _del();  // скорочена форма виклику
    }

    private static void GoodMorning() {
        Console.WriteLine("Good Morning");
    }

    private static void GoodEvening() {
        Console.WriteLine("Good Evening");
    }
}
