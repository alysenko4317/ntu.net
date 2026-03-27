using System;

class Program
{
    public class Test<T>
    {
        public T First(T[] arr) {
            return arr[0];
        }

        public object First(object[] arr) {
            return arr[0];
        }
    }

    static void Main(string[] args)
    {
        Test<string> a = new Test<string>();
        string[] s = { "str", "123" };
        Console.WriteLine("Hello World! " + a.First(s));
    }
}
