using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FriendlyTor
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

            bool onlyInstance;
            string mutexName = "123FRIENDLYTORONEAPPONLY321";
            Mutex mutex = new Mutex(true, mutexName, out onlyInstance);

            if (onlyInstance)
            {
                Application.Run(new Form1());
            }
            else
            {
                MessageBox.Show(
                   "Приложение уже запущено",
                   "Сообщение",
                   MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
    }
}
