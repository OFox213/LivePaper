using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LivePaper.Pages.SettingPages
{
    
    public partial class generalPage : Page
    {
        private const string PRODUCT_NAME = "LivePaper";
        public generalPage()
        {
            InitializeComponent();
        }

        private void setStartupRegistry(bool enabled)
        {
            ///서비스 실행을 해야함.
            Microsoft.Win32.RegistryKey startupRegistry = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (enabled) startupRegistry.SetValue(PRODUCT_NAME, "\"" + Environment.CurrentDirectory + "\"LivePaper.Service.exe");
            else startupRegistry.DeleteValue(PRODUCT_NAME, false);
        }
    }
}
