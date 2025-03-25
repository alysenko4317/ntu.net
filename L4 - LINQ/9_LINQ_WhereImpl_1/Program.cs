
using System.Collections;

public class TestSet : IEnumerable<int>
{
    public IEnumerator<int> GetEnumerator() {
        for (int i = 1; i <= 100; i++) {
            Console.WriteLine("Звернення до елементу: {0}", i);
            yield return i;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}

//----------------------------------------------------------
// MyWhere extension method impl
//----------------------------------------------------------

public static class LinqHelper
{
    public static IEnumerable<T> MyWhere<T>(this IEnumerable<T> source, Func<T, bool> predicate) {
        throw new NotImplementedException();
    }
}

//----------------------------------------------------------

class Program
{
    static void Main()
    {
        TestSet ts = new TestSet();

        var resultSet =
            ts.Where(i => i % 2 == 0)
              .Select(i => "*" + i.ToString() + "*")
              //.Where(i => i.Length == 4);
              .MyWhere(i => i.Length == 4);  // використаємо нашу власну реалізацію Where

        foreach (var item in resultSet) {
            Console.Write(item + "\n");
        }

        Console.ReadLine();
    }
}