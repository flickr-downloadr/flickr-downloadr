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
using FloydPink.Flickr.Downloadr.UI.Helpers;

namespace FloydPink.Flickr.Downloadr.UI
{
    /// <summary>
    /// Interaction logic for WarningWindow.xaml
    /// </summary>
    public partial class WarningWindow : Window
    {
        public WarningWindow()
        {
            InitializeComponent();
            Title += VersionHelper.GetVersionString();
        }

        private void ContinueButtonClick(object sender, RoutedEventArgs e) {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }
    }
}
