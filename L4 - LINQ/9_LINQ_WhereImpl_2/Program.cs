
using System.Collections;

public class TestSet : IEnumerable<int>
{
    public IEnumerator<int> GetEnumerator() {
        for (int i = 1; i <= 100; i++) {
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
        return new WhereHelperEnumerable<T>(source, predicate);
    }
}

//----------------------------------------------------------
// WhereHelperEnumerable adapter impl
//----------------------------------------------------------

public class WhereHelperEnumerable<T> : IEnumerable<T>
{
    // Приватні поля для збереження джерела даних і умови
    private readonly IEnumerable<T> _source;
    private readonly Func<T, bool> _predicate;

    // Конструктор, який приймає джерело та умову
    public WhereHelperEnumerable(IEnumerable<T> source, Func<T, bool> predicate) {
        _source = source;       // Зберігаємо джерело
        _predicate = predicate;  // Зберігаємо умову фільтрації
    }

    public IEnumerator<T> GetEnumerator() {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
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
             // .Where(i => i.Length == 4);
              .MyWhere(i => i.Length == 4);  // використаємо нашу власну реалізацію Where

        foreach (var item in resultSet) {
            Console.Write(item + " ");
        }

        Console.ReadLine();
    }
}