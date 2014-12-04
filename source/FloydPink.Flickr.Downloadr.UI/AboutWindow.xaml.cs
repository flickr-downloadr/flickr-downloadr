using System.Windows;
using FloydPink.Flickr.Downloadr.UI.Helpers;

namespace FloydPink.Flickr.Downloadr.UI {
    /// <summary>
    ///     Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window {
        public AboutWindow() {
            InitializeComponent();
            Title += VersionHelper.GetVersionString();
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
