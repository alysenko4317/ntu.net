class Program
{
    delegate int Square(int x);

    static void Main(string[] args)
    {
        Square func = i => i * i;
        // Func<int, int> func = i => i * i;  // еквівалентно
        int z = func(6);
        Console.WriteLine(z);
        Console.Read();
    }
}