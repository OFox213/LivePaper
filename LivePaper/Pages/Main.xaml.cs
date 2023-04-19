using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LivePaper.Pages
{
    /// <summary>
    /// Main.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Main : System.Windows.Controls.UserControl
    {
        List<LibWallpaperItem> list = new List<LibWallpaperItem>();
        bool isLoaded = false;
        public Main()
        {
            InitializeComponent();
            libraryWallpaper.Items.Add(new LibWallpaperItem() { Title = "test", Description = "test", WallpaperImage = "pack://application:,,,/Resources/sample.jpg" });
            libraryWallpaper.Items.Add(new LibWallpaperItem() { Title = "Sample", Description = "test", WallpaperImage = "pack://application:,,,/Resources/sample2.jpg" });
            libraryWallpaper.Items.Add(new LibWallpaperItem() { Title = "꿈 깨", Description = "test", WallpaperImage = "pack://application:,,,/Resources/sample3.jpg" });
            libraryWallpaper.Items.Add(new LibWallpaperItem() { Title = "느조스", Description = "test", WallpaperImage = "pack://application:,,,/Resources/sample4.jpg" });
            libraryWallpaper.Items.Add(new LibWallpaperItem() { Title = "샘", Description = "test", WallpaperImage = "pack://application:,,,/Resources/sample5.jpg" });
       
        }

        private void addEvent(object sender, EventArgs e)
        {
            Console.WriteLine("add eventReceived");
            libraryWallpaper.Items.Add(new LibWallpaperItem { Title = "Sample", Description = "test", WallpaperImage = "pack://application:,,,/Resources/sample2.jpg" });
        }

        private void delEvent(object sender, EventArgs e)
        {
            Console.WriteLine("delete eventReceived");
            if(libraryWallpaper.Items.Count > 0)
            {
                libraryWallpaper.Items.RemoveAt(0);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("UserControl Loaded");
            if (!isLoaded)
            {
                Console.WriteLine("UserControl First Loaded");
                isLoaded = true;
                ((MainWindow)System.Windows.Application.Current.MainWindow).AddItemHandler += new EventHandler(addEvent);
                ((MainWindow)System.Windows.Application.Current.MainWindow).DelItemHandler += new EventHandler(delEvent);
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("UserControl Unloaded");
        }

        private void openWallpaper()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "*.jpg|*.png|*.bmp";
            dialog.InitialDirectory = @"C:\";
            if(dialog.ShowDialog() == DialogResult.OK)
            {

            }
        }

        private void openWallpaperButton_Click(object sender, RoutedEventArgs e)
        {
            openWallpaper();
        }
    }

    public class LibWallpaperItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string WallpaperImage { get; set; }
    }
}
