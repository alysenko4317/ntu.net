using System;
using System.Net.Http;
using System.Windows.Forms;

namespace InterfacesDemo_3
{
    /* *** IMessage *** */

    interface IMessage {
        string ToHtml();
        string ToString();
    }

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

    /* *** IMessageSender *** */

    interface IMessageSender {
        void SendMessage(IMessage message);
    }

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


    class Program
    {
        [STAThread]
        static void Main()
        {
            IMessage textMessage = new TextMessage("This is a simple text message.");
            IMessage htmlMessage = new HtmlMessage("This is an HTML message with a custom type.", "alert");

            // Console message sender
            IMessageSender consoleSender = new ConsoleMessageSender();
            consoleSender.SendMessage(textMessage);
            consoleSender.SendMessage(htmlMessage);

            // HTMLDialog message sender
            IMessageSender dialogSender = new HtmlDialogMessageSender(new Form());
            dialogSender.SendMessage(htmlMessage); // Show HTML-formatted message in a dialog
        }
    }
}
