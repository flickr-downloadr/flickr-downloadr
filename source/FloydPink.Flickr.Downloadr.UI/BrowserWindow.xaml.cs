using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using FloydPink.Flickr.Downloadr.Bootstrap;
using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Presentation;
using FloydPink.Flickr.Downloadr.Presentation.Views;
using FloydPink.Flickr.Downloadr.UI.Extensions;

namespace FloydPink.Flickr.Downloadr.UI
{
    /// <summary>
    ///     Interaction logic for BrowserWindow.xaml
    /// </summary>
    public partial class BrowserWindow : Window, IBrowserView
    {
        private readonly BrowserPresenter _presenter;
        private ObservableCollection<Photo> _photos;

        public BrowserWindow(User user)
        {
            InitializeComponent();
            User = user;

            _presenter = Bootstrapper.GetPresenter<IBrowserView, BrowserPresenter>(this);
            _presenter.InitializeScreen();
        }

        public User User { get; set; }

        public ObservableCollection<Photo> Photos
        {
            get { return _photos; }
            set
            {
                _photos = value;
                PhotoList.DataContext = Photos;
            }
        }

        private void DownloadSelectedButtonClick(object sender, RoutedEventArgs e)
        {
            _presenter.DownloadSelected();
        }

        public void ShowSpinner(bool show)
        {
            var visibility = show ? Visibility.Visible : Visibility.Collapsed;
            Spinner.Dispatch((s) => s.Visibility = visibility);
        }

        private void DownloadAllButtonClick(object sender, RoutedEventArgs e)
        {
            _presenter.DownloadAll();
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow {User = User};
            loginWindow.Show();
            Close();
        }

        private void TogglePhotosButtonClick(object sender, RoutedEventArgs e)
        {
            var toggleButton = sender as ToggleButton;
            _presenter.TogglePhotos(toggleButton != null && (toggleButton.IsChecked ?? false));
        }
    }
}