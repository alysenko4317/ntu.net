class Program
{
    delegate int Operation(int x, int y);

    static void Main(string[] args)
    {
        Operation del = new Operation(Add);
        int result = del.Invoke(4, 5);
        Console.WriteLine(result);
        del = Multiply;
        result = del.Invoke(4, 5);
        Console.WriteLine(result);
        Console.Read();
    }

    private static int Add(int x, int y) {
        return x + y;
    }

    private static int Multiply(int x, int y) {
        return x * y;
    }
}