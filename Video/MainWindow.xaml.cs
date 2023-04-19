using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Interop;
using LivePaperFramework;

namespace Video
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _argVolume;
        private string _argPath;

        

        public MainWindow()
        {
            InitializeComponent();
            //START OF ARGUEMENT
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string[] args = Environment.GetCommandLineArgs();
            for (int i = 1; i < args.Length; i += 2)
            {
                dictionary.Add(args[i], args[i + 1]);
            }

            //Usage
            dictionary.TryGetValue("-path", out _argPath);
            dictionary.TryGetValue("-volume", out _argPath);
            //END OF ARGUEMENT

            loadProject(Environment.CurrentDirectory + "/Wallpaper/config.json");

           

            //test
            //refreshUI();
        }
        void callBack(object sender, System.Timers.ElapsedEventArgs eventArgs)
        {
            Dispatcher.Invoke(() =>
            {
                RestoreDefaultWallpaper();
                this.Close();
            });
        }

        public void changeTestLabel()
        {
            testLabel.Text = "cahnged!";
        }

        private void mainWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
            Console.WriteLine("mainWindow Closed");
        }
        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            setWallpaper();
        }
       
        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RestoreDefaultWallpaper();
            Console.WriteLine("Wallpaper is closing...");
        }
        //개쩌는 배경화면 되돌려놓는 코드
        private void RestoreDefaultWallpaper()
        {
            User32.SystemParametersInfo(User32.SPI_SETDESKWALLPAPER, 0, null, User32.SPIF_UPDATEINIFILE);
        }

        public static void RepaintWallpaper()
        {
            Console.WriteLine("Repaint");
            IntPtr desktopHandle = User32.GetDesktopWindow();
            User32.SendMessage(desktopHandle, 0x000F, IntPtr.Zero, IntPtr.Zero);
        }

        private void setWallpaper() {
            // Set Parent
            IntPtr parentHandle = GetWorkerW();
            IntPtr windowHandle = new WindowInteropHelper(this).Handle;
            User32.SetParent(windowHandle, parentHandle);

            //스크린2에 설정
            foreach (var screen in System.Windows.Forms.Screen.AllScreens)
            {
                Console.WriteLine($"Device Name : {screen.DeviceName}");
                Console.WriteLine($"Bounds : {screen.Bounds}");
                Console.WriteLine($"Bounds.Left : {screen.Bounds.Left}");
                Console.WriteLine($"Bounds.Top : {screen.Bounds.Top}");
                Console.WriteLine($"Working Area : {screen.WorkingArea}");
                Console.WriteLine($"Primary screen : {screen.Primary}");
                if (!screen.Primary)
                {
                    Console.WriteLine($"{screen.DeviceName} Screen is not primary, Adjusting window... {screen.Bounds.Width}, {screen.Bounds.Height}");
                    User32.SetWindowPos(new WindowInteropHelper(Application.Current.MainWindow).Handle, 1, screen.Bounds.Left, screen.Bounds.Top, screen.Bounds.Width, screen.Bounds.Height,
                    (int)User32.SetWindowPosFlags.SWP_NOACTIVATE);
                }
            }
        }

        private static IntPtr GetWorkerW()
        {
            //get windows wallpaper program
            IntPtr progman = GetProgman();
            UIntPtr result = UIntPtr.Zero;

            User32.SendMessageTimeout(progman, 0x052C, new UIntPtr(0), IntPtr.Zero, User32.SendMessageTimeoutFlags.SMTO_NORMAL, 1000, out result);

            IntPtr workerw = IntPtr.Zero;
            User32.EnumWindows(new User32.EnumWindowsProc((hwnd, lParam) =>
            {
                IntPtr p = User32.FindWindowEx(hwnd, IntPtr.Zero, "SHELLDLL_DefView", null);
                if (p != IntPtr.Zero)
                {
                    workerw = User32.FindWindowEx(IntPtr.Zero, hwnd, "WorkerW", null);
                }
                return true;
            }), IntPtr.Zero);
            return workerw;
        }

        private static IntPtr GetProgman()
        {
            IntPtr progman = User32.FindWindow("Progman", null);
            if(progman == IntPtr.Zero)
            {
                throw new Exception("Progman process not found");
            }
            return progman;
        }

        private void loadProject(string path)
        {
            if (File.Exists(path)) {
                StreamReader reader = new StreamReader(path);
                JObject jObject = JObject.Parse(reader.ReadToEnd());

                
                Console.WriteLine(jObject["wallpaperType"].ToString());
            } else
            {
                //파일이 존재하지 않음.
                return;
            }

        }


        private void refreshUI()
        {
            User32 user32 = new User32();

            //initialize MediaElement
            if(File.Exists(_argPath))

            mediaElement.Volume = Convert.ToDouble(_argVolume);
            mediaElement.Source = new Uri((string)_argPath);
        }
    }
}
