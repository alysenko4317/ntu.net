
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
        TestSet set = new TestSet();

        //var result = ts.Aggregate(1, (acc, x) => acc * x);

        // Фільтруємо числа, що менші або рівні 5, і знаходимо їх добуток
        var result = set.Where(i => i <= 5)
                        .Aggregate(1, (acc, i) => acc * i);

        Console.Write(result);
        Console.ReadLine();
    }
}
