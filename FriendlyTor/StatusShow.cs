using System;
using System.Windows.Forms;
using FriendlyTorCore;

namespace SettingSystemStatus
{
    public partial class StatusShow : Form
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        ProxySettings proxySettings = new ProxySettings();
        bool hotKeyPress = true;

        enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            WinKey = 8
        }

        public StatusShow()
        {
            InitializeComponent();
            SetTorSetting();

            int id = 0;     
            // The id of the hotkey. 
            RegisterHotKey(this.Handle, id, (int)KeyModifier.Control, Keys.NumPad0.GetHashCode()); 
            // Register Control + Win as global hotkey. 
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x0312)
            {
                /* Note that the three lines below are not needed if you only want to register one hotkey.
                 * The below lines are useful in case you want to register multiple keys, which you can use a switch with the id as argument, or if you want to know which key/modifier was pressed for some particular reason. */

                Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);                  // The key of the hotkey that was pressed.
                KeyModifier modifier = (KeyModifier)((int)m.LParam & 0xFFFF);       // The modifier of the hotkey that was pressed.
                int id = m.WParam.ToInt32();                                        // The id of the hotkey that was pressed.

                // do something
                if (hotKeyPress)
                {
                    SetSystemSetting();
                    hotKeyPress = false;
                }
                else
                {
                    SetTorSetting();
                    hotKeyPress = true;
                }
            }
        }

        private void SetTorSetting()
        {
            bool resultProxy = proxySettings.SetTor();
            if (resultProxy)
            {
                textBox_Status.Text = "Сайты разблокированы";

                button_Start.Visible = false;
                button_Stop.Visible = true;
                Icon = notifyIcon1.Icon = SettingSystemTor.Properties.Resources.icon_on;
            }
            else
            {
                textBox_Status.Text = "Не удалось изменить настройки!";

                button_Start.Visible = true;
                button_Stop.Visible = false;
                Icon = notifyIcon1.Icon = SettingSystemTor.Properties.Resources.icon_off;
            }
        }

        private void SetSystemSetting()
        {
            bool resultProxy = proxySettings.SetDefault();
            if (resultProxy)
            {
                textBox_Status.Text = "Сайты заблокированы";

                button_Start.Visible = true;
                button_Stop.Visible = false;
                Icon = notifyIcon1.Icon = SettingSystemTor.Properties.Resources.icon_off;
            }
            else
            {
                textBox_Status.Text = "Не удалось изменить настройки!";
                button_Start.Visible = false;
                button_Stop.Visible = true;
                Icon = notifyIcon1.Icon = SettingSystemTor.Properties.Resources.icon_on;
            }
        }

        private void button_Start_Click(object sender, EventArgs e)
        {
            SetTorSetting();
        }

        private void button_Stop_Click(object sender, EventArgs e)
        {
            SetSystemSetting();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SetSystemSetting();
            UnregisterHotKey(this.Handle, 0);       
            // Unregister hotkey with id 0 before closing the form. You might want to call this more than once with different id values if you are planning to register more than one hotkey.
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            TopMost = true;
            TopMost = false;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
            }
        }
    }
}
