
using System;
using System.Threading;

namespace ConcurrencyDemo
{
    class Program
    {
static void WriteString(object data)
{
    // для отримання рядка використовуємо перетворення типів:
    //     приводимо змінну _Data до типу string і записуємо
    //     у змінну str_for_out
    string str_for_out = (string)data;
    // тепер потік 1 тисячу разів виведе отриманий рядок (свій номер)
    for (int i = 0; i <= 1000; i++)
        Console.Write(str_for_out);
}


static void Main(string[] args)   // точка входу до програми 
{
    // створюємо 4 потоки, як параметр передаємо ім'я функції,
    //    яка буде виконуватися у створеному потоці 
    Thread th_1 = new Thread(WriteString);
    Thread th_2 = new Thread(WriteString);
    Thread th_3 = new Thread(WriteString);
    Thread th_4 = new Thread(WriteString);

    // призначаємо пріоритети для потоків 
    th_1.Priority = ThreadPriority.Highest;      // найвищій
    th_2.Priority = ThreadPriority.BelowNormal;  // вище середнього
    th_3.Priority = ThreadPriority.Normal;       // середній
    th_4.Priority = ThreadPriority.Lowest;       // низький

    // запускаємо кожен потік, як параметр передаємо номер потоку 
    th_1.Start("1");
    th_2.Start("2");
    th_3.Start("3");
    th_4.Start("4");
    Console.WriteLine("все потоки запущены ");

    // чекаємо на завершення кожного потоку 
    th_1.Join();
    th_2.Join();
    th_3.Join();
    th_4.Join();

    // прочитати символ (поки користувач не натисне клавішу програма не завершиться;
    //    це для того, щоб можна було встигнути побачити результат
    Console.ReadKey();  
}
    }
}
