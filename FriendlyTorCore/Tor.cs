using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace FriendlyTorCore
{
    public class Tor
    {
        Process process = new Process();
        readonly string changeStr = "Bootstrapped ";
        readonly string doneStr = "Bootstrapped 100%: Done";
        int processIndex = 1;
        public event EventHandler OnTorWork;
        public event EventHandler<int> OnTorStarting;

        public bool Start()
        {
            string DirectoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string filePath = Path.Combine(DirectoryName, @"TorExpert\Tor\tor.exe");

            try
            {
                process.StartInfo.FileName = filePath;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.OutputDataReceived += Process_OutputDataReceived;
                process.Start();
                process.BeginOutputReadLine();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Stop()
        {
            try
            {
                process.CancelOutputRead();
                process.Kill();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
                if (e.Data.Contains(doneStr))
                {
                    OnTorWork?.Invoke(this, e);
                    processIndex = 1;
                }
                else if (e.Data.Contains(changeStr))
                    OnTorStarting?.Invoke(this, processIndex++);
        }
    }
}
