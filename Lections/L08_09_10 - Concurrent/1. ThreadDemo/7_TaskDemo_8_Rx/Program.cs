using System;
using System.Reactive.Linq;

namespace RxTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Створюємо колекцію чисел у діапазоні від 1 до 10 за якою зможемо спостерігати.
            
            IObservable<int> range = from i in Observable.Range(1, 10)
                where i % 2 == 0   // Фільтруємо лише парні числа
                select i;

            // Підписуємося на повідомлення, Використовуємо три види обробників:
            range.Subscribe(
                // 1. Обробник кожного елемента (виводить елемент на консоль).
                i => Console.WriteLine("Елемент: {0}", i),

                // 2. Обробник помилок (виводить повідомлення про помилку на консоль).
                e => Console.WriteLine("Помилка: {0}", e.Message),

                // 3. Обробник завершення (виводить повідомлення про завершення обробки колекції).
                () => Console.WriteLine("Обхід колекції завершено.")
            );

            // Чекаємо на введення користувача, щоб програма не завершилася миттєво.
            Console.ReadLine();
        }
    }
}
