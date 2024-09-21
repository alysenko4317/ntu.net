﻿/*
    Універсальні делегати в C# — це делегати, які можуть приймати аргументи та/або повертати значення різних типів через використання узагальнень (generics). 
    Це дозволяє делегатам бути більш гнучкими та повторно використовуватись для різних типів даних.

    C# надає кілька універсальних делегатів, які є стандартними у .NET:

    1. **Func<T1, ..., TResult>**:
       - Це універсальний делегат, який може приймати до 16 вхідних параметрів і повертати результат типу `TResult`.
       - Вхідні параметри можуть мати різні типи, що дозволяє використовувати `Func` для викликів будь-яких функцій, які мають вхідні параметри і повертають значення.
       
       Приклад:
       Func<int, int, int> sum = (a, b) => a + b;
       int result = sum(3, 5); // result = 8;

    2. **Action<T1, ..., T16>**:
       - Це універсальний делегат, який може приймати до 16 параметрів, але не повертає результат (еквівалент методу `void`).
       - `Action` корисний, коли потрібно виконати певні дії з параметрами, але результат не важливий.

       Приклад:
       Action<string> printMessage = message => Console.WriteLine(message);
       printMessage("Hello, World!"); // Виводить: Hello, World!

    3. **Predicate<T>**:
       - Це спеціальний вид делегата, який приймає один параметр типу `T` і повертає логічне значення (`true` або `false`).
       - Часто використовується для перевірок умов.

       Приклад:
       Predicate<int> isPositive = number => number > 0;
       bool check = isPositive(5); // check = true;

    Універсальні делегати корисні, оскільки:
    - Вони зменшують необхідність створювати власні делегати для кожної специфічної функції.
    - Збільшують гнучкість коду, дозволяючи працювати з різними типами даних без дублювання логіки.
    - Вони легко інтегруються з лямбда-виразами та анонімними методами, що робить код більш компактним і читабельним.

    Використання універсальних делегатів дозволяє зменшити кількість зайвих інтерфейсів і забезпечити більшу гнучкість у розробці програм.
*/

class Program
{
    static void Main(string[] args)
    {
        Action<int, int> op;

        op = Add;
        Operation(10, 6, op);

        op = Subtract;
        Operation(10, 6, op);

        Console.Read();
    }

    static void Operation(int x1, int x2, Action<int, int> op)
    {
        if (x1 > x2)
            op(x1, x2);
    }

    static void Add(int x1, int x2) {
        Console.WriteLine("Сума чисел: " + (x1 + x2));
    }

    static void Subtract(int x1, int x2) {
        Console.WriteLine("Різниця чисел: " + (x1 - x2));
    }
}
