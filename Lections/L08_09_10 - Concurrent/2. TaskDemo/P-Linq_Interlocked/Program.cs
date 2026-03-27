using System;
using System.Linq;

namespace P_Linq_Interlocked
{
    class Program
    {
        //-----------------------------------------------------------------
        // P-LINQ & Interlocked
        //-----------------------------------------------------------------

        public static void Main(string[] args)
        {
            string[] cars = { "Nissan", "Aston Martin", "Chevrolet", "Alfa Romeo", "Chrysler", "Dodge", "BMW",
                              "Ferrari", "Audi", "Bentley", "Ford", "Lexus", "Mercedes", "Toyota", "Volvo",
                              "Subaru", "Жигули :)"};

            int count = 0;

            cars.AsParallel()
                .ForAll(p => {
                    if (p.Contains('s'))
                        System.Threading.Interlocked.Increment(ref count);
                });

            Console.WriteLine("Совпадений: " + count);
        }
    }
}
