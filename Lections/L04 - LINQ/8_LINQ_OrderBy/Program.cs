
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

        var resultSet =
            ts.Select(i => new {
                Number = i,
                IsEven = i % 2 == 0
            }).OrderBy(r => r.IsEven);

        foreach (var item in resultSet) {
            Console.Write(item.Number + " ");
        }

        Console.ReadLine();
    }
}