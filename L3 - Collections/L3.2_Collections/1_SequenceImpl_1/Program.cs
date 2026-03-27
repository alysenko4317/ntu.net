
using System.Collections;

public class Progression : IEnumerable<int>
{
    private readonly int _itemCount;

    public Progression(int itemCount) {
        _itemCount = itemCount;
    }

    public IEnumerator<int> GetEnumerator() {
        return new ProgressionIterator(_itemCount);
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}

public class ProgressionIterator : IEnumerator<int>
{
    private readonly int _itemCount;
    private int _position;
    private int _current;

    public ProgressionIterator(int itemCount) {
        _itemCount = itemCount;
        _current = 1;  // Початкове значення прогресії
        _position = 0; // Ініціалізуємо позицію
    }

    public bool MoveNext() {
        if (_position > 0)
            _current += 3;

        if (_position < _itemCount) {
            _position++;
            return true;  // можемо рухатись далі
        }

        return false;
    }

    public void Reset() {
        _position = 0;
        _current = 1; // Скидаємо до початкового значення
    }

    public int Current => _current;

    object IEnumerator.Current => Current;

    public void Dispose() {
        // Якщо потрібне звільнення ресурсів, можна реалізувати тут
    }
}

class Program
{
    static void Main()
    {
        var progression = new Progression(100);

        foreach (var number in progression) {
            // Виведе арифметичну прогресію 3, 6, 9 і так далі
            Console.WriteLine(number);  
        }
    }
}
