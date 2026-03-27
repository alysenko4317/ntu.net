using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        // Створити послідовний діапазон чисел
        IEnumerable<int> nums1 = Enumerable.Range(0, Int32.MaxValue);

        // Запустити секундомір
        Stopwatch sw = Stopwatch.StartNew();

        Console.WriteLine("Виконуємо запит LINQ");

        int sum1 = (from n in nums1
                    where n % 2 == 0
                    select n).Count();

        Console.WriteLine("Результат послідовного виконання: " + sum1 +
            "\nЧас: " + (sw.ElapsedMilliseconds / 1000) + " с\n");

        // Створюємо паралельний діапазон чисел
        IEnumerable<int> nums2 = ParallelEnumerable.Range(0, Int32.MaxValue);

        // Перезапускаємо секундомір
        sw.Restart();

        Console.WriteLine("Виконуємо паралельний запит LINQ");
        
        int sum2 = (from n in nums2.AsParallel()
                    where n % 2 == 0
                    select n).Count();

        Console.WriteLine("Результат паралельного виконання: " + sum2 +
            "\nЧас: " + (sw.ElapsedMilliseconds / 1000) + " с");
    }
}
