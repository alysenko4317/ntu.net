using System;
using System.Windows.Forms;

namespace WinF_Sample
{
    static class Program
    {
        ///  The main entry point for the application.

        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
