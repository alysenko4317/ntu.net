using System;
using System.Runtime.InteropServices;

//----------------------------------------------------
// Оголошення інтерфейсу для відправлення повідомлень
//----------------------------------------------------

public interface IMessageSender {
    void SendMessage(string message);
}

//---------------------------------------------------
// Різноманітні реалізації
//---------------------------------------------------

public class ConsoleMessageSender : IMessageSender
{
    // звичайний вивід у консоль
    public void SendMessage(string message) {
        Console.WriteLine("Console Message: " + message);
    }
}

public class ColorConsoleMessageSender : IMessageSender
{
    // вивід у консоль з кольором
    public void SendMessage(string message) {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Color Console Message: " + message);
        Console.ResetColor();
    }
}


public class EmailMessageSender : IMessageSender
{
    // реалізація для відправлення електронної пошти

    private string emailAddress;

    public EmailMessageSender(string email) {
        emailAddress = email;
    }

    public void SendMessage(string message) {
        Console.WriteLine("Email Message sent to " + emailAddress + ": " + message);
    }
}

public class SmsMessageSender : IMessageSender
{
    // реалізація для надсилання SMS
    
    private string phoneNumber;

    public SmsMessageSender(string phone) {
        phoneNumber = phone;
    }

    public void SendMessage(string message) {
        Console.WriteLine("SMS sent to " + phoneNumber + ": " + message);
    }
}

public class MessageBoxMessageSender : IMessageSender
{
    [DllImport("user32.dll", SetLastError = true)]
    public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

    // реалізація для виводу MessageBox (потребує Windows.Forms)

    public void SendMessage(string message) {
        MessageBox(IntPtr.Zero, "Hello from Win32 API", "Message Box", 0x00000040); // MB_OK
    }
}

//---------------------------------------------------

class Program
{
    static void SendMessage(IMessageSender sender, string message) {
        sender.SendMessage(message);
    }

    static void Main()
    {
        // Використовуємо різні реалізації IMessageSender

        SendMessage(new ConsoleMessageSender(), "Hello, World!");
        SendMessage(new ColorConsoleMessageSender(), "Hello, World!");
        SendMessage(new EmailMessageSender("example@email.com"), "Important information!");
        SendMessage(new SmsMessageSender("+1234567890"), "Emergency alert!");
        SendMessage(new MessageBoxMessageSender(), "Important message via MessageBox");
    }
}
