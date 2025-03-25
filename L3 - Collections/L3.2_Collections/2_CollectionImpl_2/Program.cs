using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class StoreCollection : ICollection<int>
{
    private readonly string _filePath;

    public StoreCollection(string filePath) {
        _filePath = filePath;
    }

    // Приватний метод для читання чисел з файлу
    private string[] GetNumbers()
    {
        // Читаємо весь вміст файлу у змінну line
        string line = File.ReadAllText(_filePath);

        // Розділяємо рядок за допомогою коми і видаляємо порожні елементи
        string[] numbers = line.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

        return numbers;
    }

    // Реалізація методу GetEnumerator
    public IEnumerator<int> GetEnumerator()
    {
        string[] numbers = GetNumbers();
        foreach (string number in numbers) {
            // Перетворюємо кожен рядок у ціле число і повертаємо його за допомогою yield return
            yield return Int32.Parse(number);
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    // Додаємо елемент до файлу
    /*public void Add(int item)
    {
        // Отримуємо поточні числа з файлу
        string[] numbers = GetNumbers();

        // Якщо файл порожній, записуємо нове число
        if (numbers.Length == 0)
        {
            File.WriteAllText(_filePath, item.ToString());
        }
        // Якщо файл не порожній, додаємо нове число через кому
        else
        {
            File.AppendAllText(_filePath, "," + item.ToString());
        }
    }*/

    void ICollection<int>.Add(int item)
    {
        // Arrays have a fixed size, so adding an item is not supported.
        throw new NotSupportedException("Collection was of a fixed size.");
    }

    public void Clear()
    {
        // Очищаємо файл, записуючи в нього порожній рядок
        File.WriteAllText(_filePath, "");
    }

    // Видаляємо елемент
    public bool Remove(int item)
    {
        string[] numbers = GetNumbers();
        string line = File.ReadAllText(_filePath);
        int symbolPosition = 0;

        foreach (string number in numbers)
        {
            if (Int32.Parse(number) == item)
            {
                if (numbers.Length == 1)
                {
                    // Якщо це єдиний елемент, видаляємо все
                    line = "";
                }
                else if (symbolPosition == 0)
                {
                    // Якщо елемент перший, видаляємо його разом з комою
                    line = line.Substring(symbolPosition + number.Length + 1);
                }
                else
                {
                    // Видаляємо елемент та кому перед ним
                    line = line.Remove(symbolPosition - 1, number.Length + 1);
                }

                // Перезаписуємо файл з оновленим вмістом
                File.WriteAllText(_filePath, line);
                return true;
            }

            // Оновлюємо позицію для наступного числа
            symbolPosition += number.Length + 1; // +1 через кому між числами
        }

        return false; // Якщо елемент не знайдено
    }

    // Перевіряємо, чи міститься елемент у колекції
    public bool Contains(int item)
    {
        // Отримуємо масив чисел з файлу
        string[] numbers = GetNumbers();

        // Проходимо через всі числа
        foreach (string number in numbers)
        {
            // Якщо знайдено число, яке збігається з item, повертаємо true
            if (Int32.Parse(number) == item)
                return true;
        }

        // Якщо число не знайдено, повертаємо false
        return false;
    }

    // Копіюємо елементи до масиву
    public void CopyTo(int[] array, int arrayIndex)
    {
        // Отримуємо масив чисел з файлу
        string[] numbers = GetNumbers();

        // Копіюємо кожне число в масив
        foreach (string number in numbers)
        {
            // Перетворюємо рядок у число і записуємо у масив
            array[arrayIndex] = Int32.Parse(number);

            // Збільшуємо індекс для наступного запису
            arrayIndex++;
        }
    }

    // Count - кількість елементів у файлі
    public int Count
    {
        get
        {
            // Отримуємо масив чисел з файлу
            string[] numbers = GetNumbers();

            // Повертаємо кількість елементів у масиві
            return numbers.Length;
        }
    }

    // Колекція не є тільки для читання
    public bool IsReadOnly => false;

    /*public bool IsReadOnly
    {
        get { return false; }
    }*/
}

class Program
{
    static void Main(string[] args)
    {
        // Створюємо новий екземпляр StoreCollection, який працює з файлом D:\test.txt
        StoreCollection collection = new StoreCollection(@"D:\_ntu\test.txt");

        // Виводимо всі числа з колекції у консоль
        // collection.Add(2014);
        foreach (int i in collection)
        {
            Console.WriteLine(i);
        }

        // Чекаємо на користувача перед закриттям програми
        Console.ReadLine();
    }
}
