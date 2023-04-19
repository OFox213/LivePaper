using LivePaper.Pages;
using ModernWpf.Media.Animation;
using System;
using System.Linq;
using System.Threading;
using System.Windows;
using Steamworks;
using System.Runtime.InteropServices;
using System.IO;
using System.Xml;
using Newtonsoft.Json.Linq;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Diagnostics;
using LivePaper.View;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using LivePaper.Core;
using LivePaperFramework;

namespace LivePaper
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private enum ProgressType : int
        {
            NORMAL = 0,
            WARNING = 1,
            ERROR = 2
        }

        [DllImport("kernel32")]
        public static extern Int32 GetCurrentProcessId();

        string previousPage = "Main";
        private SettingsWindow settingsWindow;
        private System.Timers.Timer timer;

        public event EventHandler AddItemHandler;
        public event EventHandler DelItemHandler;

        private List<ScreenLabelView> labelViews = new List<ScreenLabelView>();

        private void AddItem()
        {
            if (AddItemHandler != null)
            {
                AddItemHandler(this, EventArgs.Empty);
            }
        }

        private void DelItem()
        {
            if (DelItemHandler != null)
            {
                DelItemHandler(this, EventArgs.Empty);
            }
        }

        Main main;
        MyAsset myAsset;
        SteamWorkshop steamWorkshop;
        MonitorSettings monitorSettings;

        public MainWindow()
        {
            InitializeComponent();
            //현재 PID 저장
            Console.WriteLine("PID" + GetCurrentProcessId());
            //createFile();

            sideNav.SelectedItem = sideNav.MenuItems[0];
            main = new Main();
            myAsset = new MyAsset();
            steamWorkshop = new SteamWorkshop();
            monitorSettings = new MonitorSettings();
            contentFrame.Content = main;
            try
            {
                SteamClient.Init(480, true);
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            sideNav.UpdateLayout();

            //test

          

            var testVideo = new Video.MainWindow();
            //testVideo.Show();
            //testVideo.changeTestLabel();
        }
        //개쩌는 배경화면 되돌려놓는 코드
        private void RestoreDefaultWallpaper()
        {
            User32.SystemParametersInfo(User32.SPI_SETDESKWALLPAPER, 0, null, User32.SPIF_UPDATEINIFILE);
        }

        #region Show screen label

        private void showScreenLabel(string labelName, int x, int y, int margin) { 
            var labelView = new ScreenLabelView(labelName, x + margin, y + margin);
            labelViews.Add(labelView);
            labelView.Show();
        }
 
        private void GetScreenInformation()
        {
            foreach(var screen in Screen.AllScreens)
            {
                Console.WriteLine($"Device Name : {screen.DeviceName}");
                Console.WriteLine($"Bounds : {screen.Bounds}");
                Console.WriteLine($"Working Area : {screen.WorkingArea}");
                Console.WriteLine($"Primary screen : {screen.Primary}");
                showScreenLabel(GetScreenNumber(screen.DeviceName), screen.Bounds.Left, screen.Bounds.Top, 10);
            }
        }
        public static string GetScreenNumber(string DeviceName)
        {
            if (DeviceName == null)
                return "-1";
            var result = Regex.Match(DeviceName, @"\d+$", RegexOptions.RightToLeft);

            return result.Success ? result.Value : "-1";
        }

        private void CloseScreenNumberLabel()
        {
            if(labelViews.Count > 0)
            {
                foreach (var i in labelViews)
                {
                    i.Close();
                }
            }
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CloseScreenNumberLabel();
        }
        #endregion


        /// 더 이상 쓰이지 않는 코드이나 아카이브됨.
        public void createFile()
        {
            string folderPath = Environment.CurrentDirectory;
            Console.WriteLine($"LivePaper.UI CurrentDirectory {folderPath}");
            DirectoryInfo di = new DirectoryInfo(folderPath + "/config");
            if (di.Exists)
            {
                jsonWriterVersion();
            } else
            {
                di.Create();
                jsonWriterVersion();
            }
        }

        public void jsonWriterVersion()
        {
            JObject json = new JObject(new JProperty("currentPid",GetCurrentProcessId()));
            string path = Environment.CurrentDirectory + "/config/Program.cfg";
            File.WriteAllText(path, json.ToString());
        }
        /// End of archive code

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            initializeSteam();
        }

        private void initializeSteam()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 3000;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(callBack);
            timer.Start();
        }

        void callBack(object sender, System.Timers.ElapsedEventArgs eventArgs)
        {
            Process[] steamProcess= Process.GetProcessesByName("steam");
            if (steamProcess.Length != 0)
            {
                try
                {
                    SteamClient.Init(480, true);
                    if (SteamApps.IsSubscribed)
                    {
                        SteamIndicator.Visibility = Visibility.Collapsed;
                        timer.Stop();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Dispatcher.Invoke(() =>
                    {
                        if (e.ToString().Contains("Init but is already initialized"))
                        {
                            SteamIndicator.Visibility = Visibility.Collapsed;
                            timer.Stop();
                        }
                        else
                        {
                            SteamIndicatorText.Text = "Steam 으로 부터 확인할 수 없습니다. 재시도중...";
                            BlinkProgressbar(Convert.ToInt32(ProgressType.ERROR));
                        }
                    });
                }
            } else
            {
                Dispatcher.Invoke(() =>
                {
                    SteamIndicatorText.Text = "Steam 재연결 시도중...";
                    BlinkProgressbar(Convert.ToInt32(ProgressType.WARNING));
                });
            }
        }

        private void BlinkProgressbar(int type)
        {
            SteamIndicatorProgressBar.IsIndeterminate = false;
            SteamIndicatorProgressBar.Value = 100;
            Brush brush;
            switch (type)
            {
                case 0:
                    brush = (Brush)new BrushConverter().ConvertFrom("#FF0084FF");
                    break;
                case 1:
                    brush = (Brush)new BrushConverter().ConvertFrom("#FFFFBE30");
                    break;
                case 2:
                    brush = (Brush)new BrushConverter().ConvertFrom("#FFF33535");
                    break;
                default:
                    brush = (Brush)new BrushConverter().ConvertFrom("#FF0084FF");
                    break;
            }
            SteamIndicatorProgressBar.Foreground = brush;

            DoubleAnimation opacityAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(opacityAnimation);
            Storyboard.SetTarget(storyboard, SteamIndicatorProgressBar);
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("Opacity"));
            storyboard.Begin(SteamIndicatorProgressBar);
        }

        private void StopBlinkProgressbar()
        {
            Storyboard storyboard = new Storyboard();
            storyboard.Stop(SteamIndicatorProgressBar);
            SteamIndicatorProgressBar.Value = 0;
            SteamIndicatorProgressBar.IsIndeterminate = true;
        }

        

        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name)
               ? System.Windows.Application.Current.Windows.OfType<T>().Any()
               : System.Windows.Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
        }

        private void NavigationView_ItemInvoked(ModernWpf.Controls.NavigationView sender, ModernWpf.Controls.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            { 
                if(settingsWindow != null)
                {
                    settingsWindow.WindowState = WindowState.Normal;
                    settingsWindow.Focus();
                } else
                {
                    settingsWindow = new SettingsWindow();
                    settingsWindow.Closed += (o, ea) => settingsWindow = null;
                    settingsWindow.Show();
                }
             
                sideNav.SelectedItem = sideNav.MenuItems[0];
                sideNav.UpdateLayout();
                //WindowInteropHelper wih = new WindowInteropHelper(settings);
                //FlashWindow(wih.Handle, true);
            }
            else
            {
                string pageName = args.InvokedItemContainer.Tag.ToString();
                if (previousPage != pageName)
                {
                    previousPage = pageName;
                    
                    switch (pageName)
                    {
                        case "Main":
                            CloseScreenNumberLabel();
                            contentFrame.Content = main;
                            break;
                        case "MyAsset":
                            CloseScreenNumberLabel();
                            contentFrame.Content = myAsset;
                            break;
                        case "SteamWorkshop":
                            CloseScreenNumberLabel();
                            contentFrame.Content = steamWorkshop;
                            break;
                        case "MonitorLayout":
                            GetScreenInformation();
                            contentFrame.Content = monitorSettings;
                            break;
                    }
                   
                    
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddItem();
        }

        private void delItemButton_Click(object sender, RoutedEventArgs e)
        {
            DelItem();
        }

    }
}
