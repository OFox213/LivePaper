using LivePaper.Core;
using LivePaperFramework;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LivePaper.View
{
    /// <summary>
    /// ScreenLabelView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ScreenLabelView : Window
    {
        readonly int xPos = 0, yPos = 0;
        public ScreenLabelView(string labelText, int posLeft, int posTop)
        {
            InitializeComponent();
            ScreenLabel.Text = labelText;
            xPos = posLeft;
            yPos = posTop;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            User32.SetWindowPos(new WindowInteropHelper(this).Handle, 1, xPos, yPos, 0, 0,
           (int)User32.SetWindowPosFlags.SWP_NOACTIVATE | (int)User32.SetWindowPosFlags.SWP_NOSIZE);
        }
    }
}
