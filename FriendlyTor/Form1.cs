using System;
using System.Windows.Forms;
using FriendlyTorCore;

namespace FriendlyTor
{
    public partial class Form1 : Form
    {
        Tor tor = new Tor();
        ProxySettings proxySettings = new ProxySettings();
        bool flagRunTor = false;

        public Form1()
        {
            InitializeComponent();
            tor.OnTorWork += Tor_OnTorWork;

            button_Start.Enabled = true;
            button_Stop.Enabled = false;
        }

        private void Tor_OnTorWork(object sender, EventArgs e)
        {
            Action action = () =>
            {
                textBox_Status.Text = "Антиценз работает!";
                flagRunTor = true;

                button_Start.Enabled = false;
                button_Stop.Enabled = true;
            };

            if (InvokeRequired)
                Invoke(action);
            else
                action();
        }

        private void button_Start_Click(object sender, EventArgs e)
        {
            bool result = tor.Start();
            bool resultProxy = proxySettings.SetTor();
            if (result && resultProxy)
            {
                textBox_Status.Text = "Пытаемся запустить Антиценз";

                button_Start.Enabled = false;
                button_Stop.Enabled = false;
            }
            else
            {
                textBox_Status.Text = "Не удалось запустить Антиценз!";

                button_Start.Enabled = true;
                button_Stop.Enabled = false;
            }
        }

        private void button_Stop_Click(object sender, EventArgs e)
        {
            bool result = tor.Stop();
            bool resultProxy = proxySettings.SetDefault();
            if (result && resultProxy)
            {
                textBox_Status.Text = "Антиценз остановлен";
                flagRunTor = false;

                button_Start.Enabled = true;
                button_Stop.Enabled = false;
            }
            else
            {
                textBox_Status.Text = "Ошибка при остановке Антиценз";
                button_Start.Enabled = false;
                button_Stop.Enabled = true;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (flagRunTor)
            {
                bool result = tor.Stop();
                bool resultProxy = proxySettings.SetDefault();
                if (result && resultProxy)
                {
                    textBox_Status.Text = "Антиценз остановлен";
                    flagRunTor = false;

                    button_Start.Enabled = true;
                    button_Stop.Enabled = false;
                }
                else
                {
                    textBox_Status.Text = "Ошибка при остановке Антиценз";
                    button_Start.Enabled = false;
                    button_Stop.Enabled = true;
                }
            }
        }
    }
}
