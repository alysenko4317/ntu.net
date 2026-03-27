using System;

namespace HelloWorld
{
    class Program
    {
        static void processOption1()  // while loop demonstration
        {
            int a = 50;
            while (a > 5) {
                System.Console.Write(a + " ");
                a--;
            }

            int[,] arr_name_1 = new int[4, 2];
            int[,] arr_name_2 = {
                { 0, 1, 2, 3 },
                { 0, 1, 2, 0 }
            };

            for (int i = 0; i < arr_name_1.GetLength(0); i++) {
                for (int j = 0; j < arr_name_1.GetLength(1); j++) {
                    System.Console.WriteLine(arr_name_1[i, j]);
                }
            }
        }

        static void processOption2()  // do-while loop demonstration
        {
            int a = 50;
            do
            {
                System.Console.Write(a + " ");
                a--;
            } while (a > 5);
        }

        static void processOption3()  // for loop demonstration
        {
            for (int a = 50; a > 5; a--) {
                System.Console.Write(a + " ");
            }
        }

        static void processOption4()  // foreach loop demonstration
        {
            int[] a = new int[] { 1, 2, 3 };
            foreach (int x in a) {
                System.Console.Write(x + " ");
            }
        }

        static void Main(string[] args)
        {
            bool exit = false;
            int invalidChoises = 0;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Main Menu");
                Console.WriteLine("  1. Option 1 - while loop demonstration");
                Console.WriteLine("  2. Option 2 - do-while loop demonstration");
                Console.WriteLine("  3. Option 3 - for loop demonstration");
                Console.WriteLine("  4. Option 4 - foreach loop demonstration");
                Console.WriteLine("  5. Exit");
                Console.WriteLine();
                Console.WriteLine("Enter your choice:");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.WriteLine("Option 1 selected.");
                        processOption1();
                        break;
                    case "2":
                        Console.WriteLine("Option 2 selected.");
                        processOption2();
                        break;
                    case "3":
                        Console.WriteLine("Option 3 selected.");
                        processOption3();
                        break;
                    case "4":
                        Console.WriteLine("Option 4 selected.");
                        processOption4();
                        break;
                    case "5":
                        exit = true;
                        break;
                    default:

                        Console.WriteLine("Invalid choice. Please try again.");
                        if (++invalidChoises > 5) { 
                            exit = true;   // too many invalid choises done
                        }
                        break;
                }

                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }

            Console.WriteLine("Exiting the program. Press any key to close the application.");
            Console.ReadKey();
        }
    }
}
