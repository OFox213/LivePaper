using Steamworks;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace LivePaper.Pages.SettingPages
{
    public partial class InformationPage : Page
    {
        private string EmailAddress = "skyfoxstudio213@gmail.com";
        public InformationPage()
        {
            InitializeComponent();
            initializeUI();
        }

        private async void initializeUI()
        {
            AssemblyName name = new AssemblyName(Assembly.GetExecutingAssembly().FullName);
            versionText.Text = $"Latest BuildVersion {name.Version}";
            contactButton.Content = $"{Properties.Resources.Contact} | {EmailAddress}";

            if (SteamClient.IsValid)
            {
                if (SteamClient.IsLoggedOn)
                {
                    var steamId = SteamClient.SteamId;
                    var image = await SteamFriends.GetLargeAvatarAsync(steamId);
                    //Steam Avatar
                    var resultImage = image.Value;
                    if (resultImage.Data != null)
                    {
                        int imageWidth = (int)resultImage.Width;
                        int imageHeight = (int)resultImage.Height;
                        Bitmap bi = new Bitmap(imageWidth, imageHeight);
                        for (int y = 0; y < imageHeight; y++)
                        {
                            for (int x = 0; x < imageWidth; x++)
                            {
                                byte r = resultImage.GetPixel(x, y).r;
                                byte g = resultImage.GetPixel(x, y).g;
                                byte b = resultImage.GetPixel(x, y).b;
                                byte a = resultImage.GetPixel(x, y).a;
                                Color color = Color.FromArgb(a, r, g, b);
                                bi.SetPixel(x, y, color);
                            }
                        }
                        MemoryStream ms = new MemoryStream();
                        bi.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        var bitmapI = new BitmapImage();
                        bitmapI.BeginInit();
                        bitmapI.StreamSource = ms;
                        bitmapI.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapI.EndInit();
                        SteamProfileAvatar.Source = bitmapI;

                        Console.WriteLine(resultImage.Height);
                        Console.WriteLine(resultImage.Width);
                    }
                    else
                    {
                        SteamProfileAvatar.Visibility = Visibility.Collapsed;
                        SteamProfileNotFound.Visibility = Visibility.Visible;
                        Console.WriteLine("Avatar is not found.");
                    }
                    //Steam Name and ID
                    SteamNameText.Text = $"{Steamworks.SteamClient.Name}";
                    SteamIdText.Text = $"Steam ID | {steamId}";
                }
                else
                {
                    //Steam is not logged in
                    SteamProfileAvatar.Visibility = Visibility.Collapsed;
                    SteamProfileNotFound.Visibility = Visibility.Visible;
                    SteamNameText.Text = "Could not load info from steam";
                    SteamIdText.Text = "Steam offline";
                }
            }
            var licensePath = Environment.CurrentDirectory + "/license";
            if (File.Exists(licensePath))
            {
                licenseText.Text = File.ReadAllText(licensePath);
            }
           
        }

        private void contactButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start($"mailto:{EmailAddress}");
        }
    }
}
