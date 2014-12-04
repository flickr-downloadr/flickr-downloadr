using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using FloydPink.Flickr.Downloadr.Bootstrap;
using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Model.Extensions;
using FloydPink.Flickr.Downloadr.Presentation;
using FloydPink.Flickr.Downloadr.Presentation.Views;
using FloydPink.Flickr.Downloadr.UI.Extensions;
using FloydPink.Flickr.Downloadr.UI.Helpers;
using MessageBox = System.Windows.MessageBox;

namespace FloydPink.Flickr.Downloadr.UI {
    /// <summary>
    ///     Interaction logic for PreferencesWindow.xaml
    /// </summary>
    public partial class PreferencesWindow : Window, IPreferencesView, INotifyPropertyChanged {
        private readonly IPreferencesPresenter _presenter;
        private Preferences _preferences;

        public PreferencesWindow(User user, Preferences preferences) {
            InitializeComponent();
            Title += VersionHelper.GetVersionString();
            Preferences = preferences;
            User = user;

            this._presenter = Bootstrapper.GetPresenter<IPreferencesView, IPreferencesPresenter>(this);

            SetCacheSize();
        }

        protected User User { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public Preferences Preferences {
            get { return this._preferences; }
            set {
                this._preferences = value;
                PropertyChanged.Notify(() => Preferences);
            }
        }

        public void ShowSpinner(bool show) {
            Visibility visibility = show ? Visibility.Visible : Visibility.Collapsed;
            this.Spinner.Dispatch(s => s.Visibility = visibility);
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e) {
            this._presenter.Save(Preferences);
            var browserWindow = new BrowserWindow(User, Preferences);
            browserWindow.Show();
            Close();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e) {
            var loginWindow = new LoginWindow(User);
            loginWindow.Show();
            Close();
        }

        private void SelectDownloadFolderButtonClick(object sender, RoutedEventArgs e) {
            var dialog = new FolderBrowserDialog {
                Description = "Select folder to save downloaded photos:",
                SelectedPath = Preferences.DownloadLocation
            };
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK) {
                Preferences.DownloadLocation = dialog.SelectedPath;
            }
        }

        private void SelectCacheFolderButtonClick(object sender, RoutedEventArgs e) {
            var dialog = new FolderBrowserDialog {
                Description = "Select folder to save the cached thumbnails:",
                SelectedPath = Preferences.CacheLocation
            };
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK) {
                Preferences.CacheLocation = dialog.SelectedPath;
            }
        }

        private void DefaultsButtonClick(object sender, RoutedEventArgs e) {
            Preferences = Preferences.GetDefault();
        }

        private void EmptyCacheClick(object sender, RoutedEventArgs e) {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to empty the cache folder?",
                "Please confirm...", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes) {
                this._presenter.EmptyCacheDirectory(Preferences.CacheLocation);
                SetCacheSize();
            }
        }

        private void SetCacheSize() {
            this.CacheSize.Content = this._presenter.GetCacheFolderSize(Preferences.CacheLocation);
            this.EmptyCacheButton.Visibility = (this.CacheSize.Content.ToString() == "0 B" || this.CacheSize.Content.ToString() == "-")
                ? Visibility.Collapsed
                : Visibility.Visible;
        }
    }
}
