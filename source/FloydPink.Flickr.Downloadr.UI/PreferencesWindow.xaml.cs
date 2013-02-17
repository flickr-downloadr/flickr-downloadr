using System.Windows;
using System.Windows.Forms;
using FloydPink.Flickr.Downloadr.Bootstrap;
using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Presentation;
using FloydPink.Flickr.Downloadr.Presentation.Views;
using FloydPink.Flickr.Downloadr.UI.Extensions;

namespace FloydPink.Flickr.Downloadr.UI
{
    /// <summary>
    ///     Interaction logic for PreferencesWindow.xaml
    /// </summary>
    public partial class PreferencesWindow : Window, IPreferencesView
    {
        private readonly IPreferencesPresenter _presenter;
        public Preferences Preferences { get; set; }
        protected User User { get; set; }

        public PreferencesWindow(User user, Preferences preferences)
        {
            Preferences = preferences;
            User = user;
            InitializeComponent();

            _presenter = Bootstrapper.GetPresenter<IPreferencesView, IPreferencesPresenter>(this);
        }

        public void ShowSpinner(bool show)
        {
            Visibility visibility = show ? Visibility.Visible : Visibility.Collapsed;
            Spinner.Dispatch((s) => s.Visibility = visibility);
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            _presenter.Save(Preferences);
            OpenBrowserWindow();
        }

        private void OpenBrowserWindow()
        {
            var browserWindow = new BrowserWindow(User, Preferences);
            browserWindow.Show();
            Close();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            OpenBrowserWindow();
        }

        private void SelectFolderButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog
                             {
                                 Description = "Select folder to save downloaded photos:",
                                 SelectedPath = Preferences.DownloadLocation
                             };
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Preferences.DownloadLocation = dialog.SelectedPath;
            }
        }
    }
}