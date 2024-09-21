
using System.Collections;

public class Progression : IEnumerable<int>
{
    private readonly int _itemCount;

    public Progression(int itemCount) {
        _itemCount = itemCount;
    }
    
    public IEnumerator<int> GetEnumerator()
    {
        int current = 1;

        for (int i = 0; i < _itemCount - 1; i++)
        {
            if (i == 0)
                yield return 1; // Повертаємо перший елемент прогресії

            current += 3; // Додаємо 3 на кожному наступному кроці
            yield return current; // Повертаємо поточне значення
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
        var progression = new Progression(100);

        foreach (var number in progression) {
            Console.WriteLine(number);  // Виведе арифметичну прогресію 3, 6, 9 і так далі
        }
    }
}
