using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace GLMinApp
{
    static class Program
    {
        //[DllImport("freeglut.dll", CallingConvention = CallingConvention.Cdecl)]

        //public static extern void glutInit(int argc, string[] argv);
        //public static extern void glutInitDisplayMode(uint mode);

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
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
