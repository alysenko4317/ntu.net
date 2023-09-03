using System;

class KeyEventArgs : EventArgs
{
    public char KeyChar { get; set; }
}

class KeyEvent
{
    public event EventHandler<KeyEventArgs> KeyPress;

    public void OnKeyPress(char key)
    {
        KeyEventArgs e = new KeyEventArgs { KeyChar = key };
        KeyPress?.Invoke(this, e);
    }
}

// Замикання (closures) - це концепція в програмуванні, яка визначає те, як функція зберігає доступ до змінних
// з оточуючого контексту, в якому вона була створена, навіть після того, як цей контекст вже закінчив свою роботу.

class KeyEventDemo
{
    static void Main()
    {
        int count = 0;
        ConsoleKeyInfo key;  // інформація про натиснуті клавіші

        KeyEvent kevt = new KeyEvent();

        kevt.KeyPress += (sender, e) => {
            Console.WriteLine("Натиснуто клавішу: " + e.KeyChar);
            count++;

            // використання змінної count у лямбда-виразі(sender, e) => { ... }
            // є прикладом замикання в C#
        };

        Console.WriteLine(
            "Будь ласка, натискайте клавіші. Для завершення введіть крапку '.'");

        do
        {
            key = Console.ReadKey();
            kevt.OnKeyPress(key.KeyChar);
        }
        while (key.KeyChar != '.');

        Console.WriteLine("Ви натиснули " + count + " клавіш.");
        Console.ReadKey();
    }
}


