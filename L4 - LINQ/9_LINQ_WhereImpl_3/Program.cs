
using System.Collections;

public class TestSet : IEnumerable<int>
{
    // Реалізація методу GetEnumerator для типу IEnumerable<int>
    public IEnumerator<int> GetEnumerator()
    {
        for (int i = 1; i <= 100; i++)
        {
            Console.WriteLine("Обращение к элементу: {0}", i);
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
    public static IEnumerable<T> MyWhere<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        return new WhereHelperEnumerable<T>(source, predicate);
    }
}

//----------------------------------------------------------
// WhereHelperEnumerable adapter impl
//----------------------------------------------------------

public class WhereHelperEnumerable<T> : IEnumerable<T>
{
    private readonly IEnumerable<T> _source;
    private readonly Func<T, bool> _predicate;

    public WhereHelperEnumerable(IEnumerable<T> source, Func<T, bool> predicate)
    {
        _source = source;
        _predicate = predicate;
    }

    public IEnumerator<T> GetEnumerator()
    {
        IEnumerator<T> sourceEnumerator = _source.GetEnumerator();
        return new WhereHelperEnumerator<T>(sourceEnumerator, _predicate);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

//----------------------------------------------------------
// Iterator with filtration impl
//----------------------------------------------------------

public class WhereHelperEnumerator<T> : IEnumerator<T>
{
    private readonly IEnumerator<T> _sourceEnumerator;
    private readonly Func<T, bool> _predicate;

    // Конструктор приймає джерело Enumerator і умову predicate
    public WhereHelperEnumerator(IEnumerator<T> sourceEnumerator, Func<T, bool> predicate)
    {
        _sourceEnumerator = sourceEnumerator;
        _predicate = predicate;
    }

    // Реалізація методу MoveNext для ітерації
    public bool MoveNext()
    {
        bool isValid = false;

        // Перебір елементів джерела, поки MoveNext() повертає true
        while (_sourceEnumerator.MoveNext())
        {
            // Отримуємо поточний елемент
            T current = _sourceEnumerator.Current;

            // Перевіряємо, чи відповідає поточний елемент умові
            isValid = _predicate(current);

            // Якщо елемент відповідає умові, зупиняємо цикл
            if (isValid) break;
        }

        // Повертаємо результат: знайдений елемент або немає
        return isValid;
    }

    // Скидання Enumerator до початку
    public void Reset() {
        _sourceEnumerator.Reset();
    }

    // Повертає поточний елемент, який задовольняє умові
    public T Current {
        get { return _sourceEnumerator.Current; }
    }

    // Неузагальнена версія Current
    object IEnumerator.Current {
        get { return Current; }
    }

    // Звільнення ресурсів
    public void Dispose() {
        _sourceEnumerator.Dispose();
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