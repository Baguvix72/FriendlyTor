using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace FriendlyTorCore
{
    public class ProxySettings
    {
        //Эти поля нужны для обновления глобальных настроек прокси
        [DllImport("wininet.dll")]
        static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
        const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        const int INTERNET_OPTION_REFRESH = 37;
        static bool settingsReturn, refreshReturn;

        object proxyEnableSetting;
        object proxyServerSetting;

        public bool SetTor()
        {
            try
            {
                RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);

                proxyEnableSetting = registry.GetValue("ProxyEnable");
                proxyServerSetting = registry.GetValue("ProxyServer");

                registry.SetValue("ProxyEnable", 1);
                registry.SetValue("ProxyServer", "socks=127.0.0.1:9050");

                //Обновление настроек прокси
                settingsReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
                refreshReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SetDefault()
        {
            try
            {
                RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);

                registry.SetValue("ProxyEnable", proxyEnableSetting);
                registry.SetValue("ProxyServer", proxyServerSetting);

                //Обновление настроек прокси
                settingsReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
                refreshReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
