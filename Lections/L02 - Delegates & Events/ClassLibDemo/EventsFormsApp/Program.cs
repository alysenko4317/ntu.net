using MathSolverClassLibrary;
using System.Windows.Forms;

namespace EventsFormsApp
{
    interface IMessage
    {
        string ToHtml();
        string ToString();
        void WriteLine(string txt);
    }

    class HtmlMessage : IMessage
    {
        private string _msg;
        private string _type;

        public HtmlMessage(string msg, string type) {
            _msg = msg + "<br>";
            _type = type;
        }

        public void WriteLine(string txt) {
            _msg += txt + "<br>";
        }

        public string ToHtml() {
            string color = GetColorForMessageType(_type);
            return $"<html><body><br><br><h1><font color='{color}'><center><b>{_msg}</b></center></font></h1></body></html>";
        }

        public string ToString() {
            return $"[{_type}] {_msg}";
        }

        private string GetColorForMessageType(string type)
        {
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

    //------------------------------------------------------

    interface IMessageSender {
        void SendMessage(IMessage message);
    }

    class ConsoleMessageSender : IMessageSender {
        public void SendMessage(IMessage message)
        {
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
            _form.Size = new System.Drawing.Size(800, 300); // Set the desired form size
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

    //------------------------------------------------------

    class Program
    {
        [STAThread]
        static void Main()
        {
            IMessage htmlMessage = new HtmlMessage("MathSolver DEMO", "");

            MathSolver solver = new MathSolver(-1, 1, 0.2);

            solver.InitializeEvent += (sender, args) => {
                htmlMessage.WriteLine($"<font color='green'>xmin</font>={args.xmin} <font color='green'>xmax</font>={args.xmax} <font color='blue'>step</font>={args.step}");
            };

            solver.ProcessingStartedEvent += (sender, args) => {
                htmlMessage.WriteLine("<font color='brown'>ProcessingStartedEvent</font> received");
            };

            solver.ProcessingFinishedEvent += (sender, args) => {
                htmlMessage.WriteLine($"result=<font color='red'>{args.result}</font>");
            };

            double result = solver.Solve(x => x * x);

            IMessageSender dialogSender = new HtmlDialogMessageSender(new Form());
            dialogSender.SendMessage(htmlMessage);  // Show HTML-formatted message in a dialog
        }
    }
}