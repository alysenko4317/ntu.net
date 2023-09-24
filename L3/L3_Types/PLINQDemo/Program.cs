using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        // Создать последовательный диапазон чисел
        IEnumerable<int> nums1 = Enumerable.Range(0, Int32.MaxValue);

        // Запустить секундомер
        Stopwatch sw = Stopwatch.StartNew();

        Console.WriteLine("Выполняем запрос LINQ");

        int sum1 = (from n in nums1
                    where n % 2 == 0
                    select n).Count();

        Console.WriteLine("Результат последовательного выполнения: " + sum1 +
            "\nВремя: " + (sw.ElapsedMilliseconds / 1000) + " с\n");

        // Создаем параллельный диапазон чисел
        IEnumerable<int> nums2 = ParallelEnumerable.Range(0, Int32.MaxValue);

        // Перезапускаем секундомер
        sw.Restart();

        Console.WriteLine("Выполняем параллельный запрос LINQ");
        
        int sum2 = (from n in nums2.AsParallel()
                    where n % 2 == 0
                    select n).Count();

        Console.WriteLine("Результат параллельного выполнения: " + sum2 +
            "\nВремя: " + (sw.ElapsedMilliseconds / 1000) + " с");
    }
}
