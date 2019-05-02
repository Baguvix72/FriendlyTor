using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace SettingSystemStatus
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
            string mutexName = "123SETTINGSYSTEMTORCHANGER02022019321";
            Mutex mutex = new Mutex(true, mutexName, out onlyInstance);

            if (onlyInstance)
            {
                Application.Run(new StatusShow());
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
