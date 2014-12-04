using System.Windows;
using FloydPink.Flickr.Downloadr.UI.Helpers;

namespace FloydPink.Flickr.Downloadr.UI {
    /// <summary>
    ///     Interaction logic for WarningWindow.xaml
    /// </summary>
    public partial class WarningWindow : Window {
        public WarningWindow() {
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
