using System;
using System.Windows.Forms;

namespace HandyBoxApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //todo: make it singleton
            MainForm mainForm = new MainForm();
            Application.Run(new CustomApplicationContext(mainForm));
        }
    }
}
