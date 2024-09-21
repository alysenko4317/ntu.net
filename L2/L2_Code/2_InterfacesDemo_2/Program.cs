using System;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Windows.Forms;

/*
Ця програма демонструє концепцію слабкої зв'язаності в об'єктно-орієнтованому програмуванні (ООП) через використання двох інтерфейсів:
1. IMessage - абстракція повідомлення, яка дозволяє використовувати різні форми повідомлень (текстові, HTML тощо).
2. IMessageSender - абстракція відправника повідомлень, що дозволяє надсилати повідомлення через різні канали (консоль, діалогові вікна тощо).

Основні переваги даної архітектури:
- **Слабка зв'язаність**: 
  Клас Program не прив'язаний до конкретних реалізацій повідомлень або способів їх відправки. Це дозволяє легко змінювати логіку 
  надсилання повідомлень або сам формат повідомлень без необхідності вносити зміни в основний код.
- **Розширюваність**: 
  Для додавання нових типів повідомлень (наприклад, повідомлення у форматі JSON) або нових способів їх відправки (наприклад, через HTTP) 
  достатньо створити нові класи, що реалізують інтерфейси IMessage та IMessageSender відповідно. Основна логіка залишиться незмінною.
- **Інверсія залежностей**: 
  Програма залежить не від конкретних класів реалізацій, а від абстракцій (інтерфейсів). Це відповідає принципу інверсії залежностей 
  (Dependency Inversion Principle) з SOLID, що робить код більш гнучким і менш залежним від конкретних реалізацій.

У цій програмі:
- **IMessage** реалізує два різних типи повідомлень: `TextMessage` (простий текст) та `HtmlMessage` (HTML-форматоване повідомлення).
  Кожен тип повідомлення має два методи: `ToString()` для текстового представлення та `ToHtml()` для HTML-представлення.
- **IMessageSender** реалізує дві різні стратегії відправки повідомлень:
  - `ConsoleMessageSender` виводить повідомлення у текстовому вигляді в консоль.
  - `HtmlDialogMessageSender` відображає HTML-форматовані повідомлення у діалоговому вікні з використанням Windows Forms.

Така архітектура дозволяє легко змінювати або розширювати як способи відправки повідомлень, так і формати самих повідомлень,
не змінюючи основну структуру програми.
*/

namespace InterfacesDemo_3
{
    //---------------------------------------------------
    /* *** IMessage *** */
    //---------------------------------------------------

    interface IMessage {
        string ToHtml();
        string ToString();
    }

    //---------------------------------------------------

    class TextMessage : IMessage
    {
        private string _msg;

        public TextMessage(string msg) {
            _msg = msg;
        }

        public string ToHtml() {
            return _msg;
        }

        public string ToString() {
            return _msg;
        }
    }

    class HtmlMessage : IMessage
    {
        private string _msg;
        private string _type;

        public HtmlMessage(string msg, string type) {
            _msg = msg;
            _type = type;
        }

        public string ToHtml() {
            string color = GetColorForMessageType(_type);
            return $"<html><body><br><br><h1><font color='{color}'><center><b>{_msg}</b></center></font></h1></body></html>";
        }

        public string ToString() {
            return $"[{_type}] {_msg}";
        }

        private string GetColorForMessageType(string type) {
            switch (type.ToLower())
            {
                case "alert":
                    return "brown";
                case "error":
                    return "red";
                case "ok":
                    return "green";
                default:
                    return "black";
            }
        }
    }

    //---------------------------------------------------
    /* *** IMessageSender *** */
    //---------------------------------------------------

    interface IMessageSender {
        void SendMessage(IMessage message);
    }

    //---------------------------------------------------

    class ConsoleMessageSender : IMessageSender
    {
        public void SendMessage(IMessage message) {
            Console.WriteLine(message.ToString());
        }
    }

    class HtmlDialogMessageSender : IMessageSender
    {
        private Form _form;

        public HtmlDialogMessageSender(Form form)
        {
            _form = form;
            InitializeForm();
        }

        private void InitializeForm()
        {
            _form.Text = "HTML Message Dialog";
            _form.Size = new System.Drawing.Size(800, 200); // Set the desired form size
            _form.StartPosition = FormStartPosition.CenterScreen; // Center the form on the screen

            WebBrowser browser = new WebBrowser();
            browser.Dock = DockStyle.Fill; // Fill the entire form with the WebBrowser control
            browser.ScrollBarsEnabled = false; // Disable scroll bars
            browser.WebBrowserShortcutsEnabled = false; // Disable keyboard shortcuts

            _form.Controls.Add(browser);
        }

        public void SendMessage(IMessage message)
        {
            if (_form is Form form)
            {
                WebBrowser browser = (WebBrowser)form.Controls[0];
                browser.DocumentText = message.ToHtml();
                form.ShowDialog();
            }
        }
    }

    //---------------------------------------------------
    // Main
    //---------------------------------------------------

    class Program
    {
        // Оголошення зовнішньої функції для створення консолі
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [STAThread]
        static void Main()
        {
            AllocConsole();

            IMessage textMessage = new TextMessage("This is a simple text message.");
            IMessage htmlMessage = new HtmlMessage("This is an HTML message with a custom type.", "alert");

            // Console message sender
            IMessageSender consoleSender = new ConsoleMessageSender();
            consoleSender.SendMessage(textMessage);
            consoleSender.SendMessage(htmlMessage);

            // Залишаємо консоль видимою перед відкриттям вікна
            Console.ReadKey();

            // HTMLDialog message sender
            IMessageSender dialogSender = new HtmlDialogMessageSender(new Form());
            dialogSender.SendMessage(htmlMessage); // Show HTML-formatted message in a dialog
        }
    }
}
