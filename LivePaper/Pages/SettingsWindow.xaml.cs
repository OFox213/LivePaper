using LivePaper.Pages.SettingPages;
using ModernWpf.Media.Animation;
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
using System.Windows.Shapes;

namespace LivePaper.Pages
{
    /// <summary>
    /// SettingsWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private static SettingsWindow instance = null;
        private bool isLoaded = false;

        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void FormLoaded(object sender, RoutedEventArgs e)
        {
            isLoaded = true;

            settingsFrame.Navigate(typeof(InformationPage), null, new DrillInNavigationTransitionInfo());
            //settingsFrame.Navigate(typeof(Main));
        }

        private void SettingsTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isLoaded)
            {
                var currentIndex = (sender as TabControl).SelectedIndex;
                switch (currentIndex)
                {
                    case 0:
                        settingsFrame.Navigate(typeof(Main), null, new DrillInNavigationTransitionInfo());
                        break;
                    case 1:
                        settingsFrame.Navigate(typeof(MyAsset), null, new DrillInNavigationTransitionInfo());
                        break;
                    case 2:
                        settingsFrame.Navigate(typeof(generalPage), null, new DrillInNavigationTransitionInfo());
                        break;
                    case 3:
                        settingsFrame.Navigate(typeof(generalPage), null, new DrillInNavigationTransitionInfo()); //나중에 수정
                        break;
                    case 4:
                        settingsFrame.Navigate(typeof(InformationPage), null, new DrillInNavigationTransitionInfo());
                        break;
                }
            }
         
        }
    }
}
