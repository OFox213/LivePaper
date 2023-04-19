using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LivePaper;
using LivePaperFramework;
using Newtonsoft.Json.Linq;

namespace LivePaper.Service
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            setTrayIcon();
            startUI();
            this.Hide();
            
        }

        private void startUI()
        {
            Process mainProcess = new Process();
            mainProcess.StartInfo.FileName = "LivePaper.UI.exe";
            mainProcess.Start();
        }


        private void setTrayIcon()
        {
            System.Windows.Forms.ContextMenu menu = new System.Windows.Forms.ContextMenu();
            System.Windows.Forms.MenuItem openItem = new System.Windows.Forms.MenuItem();
            openItem.Index = 0;
            openItem.Text = "열기";
            openItem.Click += delegate (object sender, EventArgs e)
            {
                openUI();
            };

            menu.MenuItems.Add(openItem);

            System.Windows.Forms.MenuItem exitItem = new System.Windows.Forms.MenuItem();
            exitItem.Index = 0;
            exitItem.Text = "LivePaper 종료";
            exitItem.Click += delegate (object sender, EventArgs e)
            {
                CloseOtherProccess("LivePaper.UI");
                CloseOtherProccess("LivePaper.Video");
                

                Environment.Exit(0);
            };
            
            menu.MenuItems.Add(exitItem);

            System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.DoubleClick += delegate (object sender, EventArgs e) {
                openUI();
            };
            notifyIcon.Icon = Properties.Resources.icon;
            notifyIcon.Visible = true;
            notifyIcon.ContextMenu = menu;
            notifyIcon.Text = "LivePaper";
            //notifyIcon.DoubleClick += delegate(object sender, EventArgs e) {  }
        }

        private void CloseOtherProccess(string processName)
        {
            Process[] otherProcess = Process.GetProcessesByName(processName);
            if (otherProcess.Length != 0)
            {
                foreach (Process process in otherProcess)
                {
                    process.CloseMainWindow();
                }
            }
        }

        private void openUI()
        {
            Process[] processes = Process.GetProcessesByName("LivePaper.UI");
            if(processes.Length == 0)
            {
                startUI();
            } else
            {
                var handle = processes[0].MainWindowHandle;
                if (User32.IsIconic(handle))
                    User32.ShowWindow(handle, 9);
                User32.SetForegroundWindow(handle);
            }
        }


        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name)
               ? Application.Current.Windows.OfType<T>().Any()
               : Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
