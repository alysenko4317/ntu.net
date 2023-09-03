using System;

class Program
{
    static void Main(string[] args)
    {
        Func<int, int> retFunc = Factorial;
        int n1 = GetPositive(6, retFunc);
        Console.WriteLine(n1);  // 720

        int n2 = GetPositive(6, x => x * x);
        Console.WriteLine(n2);  // 36

        Console.Read();
    }

    static int GetPositive(int x1, Func<int, int> retF)
    {
        int result = 0;
        if (x1 > 0) 
            result = retF(x1);
        return result;
    }

    static int Factorial(int x)
    {
        int result = 1;
        for (int i = 1; i <= x; i++)
            result *= i;
        return result;
    }
}