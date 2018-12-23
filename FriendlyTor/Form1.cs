using System;
using System.Windows.Forms;
using FriendlyTorCore;

namespace FriendlyTor
{
    public partial class Form1 : Form
    {
        Tor tor = new Tor();

        public Form1()
        {
            InitializeComponent();
            tor.OnTorWork += Tor_OnTorWork;
        }

        private void Tor_OnTorWork(object sender, EventArgs e)
        {
            Action action = () =>
            {
                label_Status.Text = "Антиценз работает!";
            };

            if (InvokeRequired)
                Invoke(action);
            else
                action();
        }

        private void button_Start_Click(object sender, EventArgs e)
        {
            bool result = tor.Start();
            if (result)
                label_Status.Text = "Пытаемся запустить Антиценз";
            else
                label_Status.Text = "Не удалось запустить Антиценз!";
        }

        private void button_Stop_Click(object sender, EventArgs e)
        {
            bool result = tor.Stop();
            if (result)
                label_Status.Text = "Антиценз остановлен";
            else
                label_Status.Text = "Ошибка при остановке Антиценз";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool result = tor.Stop();
            if (result)
                label_Status.Text = "Антиценз остановлен";
            else
                label_Status.Text = "Ошибка при остановке Антиценз";
        }
    }
}
