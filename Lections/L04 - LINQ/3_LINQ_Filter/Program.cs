
using System.Collections;

public class TestSet : IEnumerable<int>
{
    // Реалізація методу GetEnumerator для типу IEnumerable<int>
    public IEnumerator<int> GetEnumerator() {
        for (int i = 1; i <= 100; i++) {
            yield return i;
        }
    }

    // Явна реалізація методу GetEnumerator для інтерфейсу IEnumerable
    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator(); // Виклик узагальненого методу GetEnumerator
    }
}

class Program
{
    static void Main()
    {
        TestSet ts = new TestSet();

        // Використовуємо LINQ
        var resultSet = ts.Where<int>(s => s > 50);

        foreach (var item in resultSet) {
            Console.Write(item + " ");
        }

        Console.ReadLine();
    }
}
