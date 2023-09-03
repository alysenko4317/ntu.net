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
