
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

class Program
{
    static void Main()
    {
        TestSet ts = new TestSet();

        // Використовуємо LINQ
        var resultSet = ts.Select(s => "*" + s.ToString() + "*");

        foreach (var item in resultSet) {
            Console.Write(item + " ");
        }

        Console.ReadLine();
    }
}
