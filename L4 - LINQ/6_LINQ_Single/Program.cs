using System;
//using System.Linq;
using System.Collections.Generic;
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
        
        var result = ts.FirstOrDefault(s => s % 2 == 0);
        //var result = ts.SingleOrDefault(s => s % 2 == 0);

        Console.Write(result);
        Console.ReadLine();
    }
}
