using FloydPink.Flickr.Downloadr.Bootstrap;
using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Presentation;
using FloydPink.Flickr.Downloadr.Presentation.Views;
using FloydPink.Flickr.Downloadr.UI.Extensions;
using log4net;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace FloydPink.Flickr.Downloadr.UI
{
    /// <summary>
    ///     Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window, ILoginView
    {
        private readonly LoginPresenter _presenter;
        private User _user;

        private static readonly ILog Log = LogManager.GetLogger(typeof(LoginWindow));

        public LoginWindow()
            : this(new User())
        {
        }

        public LoginWindow(User user)
        {
            InitializeComponent();

            User = user;

            _presenter = Bootstrapper.GetPresenter<ILoginView, LoginPresenter>(this);
            _presenter.InitializeScreen();
        }

        #region ILoginView Members

        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                SetWelcomeLabel(value);
            }
        }

        public void ShowLoggedInControl()
        {
            LoggedInCanvas.Dispatch((c) => c.Visibility = Visibility.Visible);
            LoggedOutCanvas.Dispatch((c) => c.Visibility = Visibility.Collapsed);
        }

        public void ShowLoggedOutControl()
        {
            LoggedOutCanvas.Dispatch((c) => c.Visibility = Visibility.Visible);
            LoggedInCanvas.Dispatch((c) => c.Visibility = Visibility.Collapsed);
        }

        #endregion

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            _presenter.Login();
        }

        private void LogoutButtonClick(object sender, RoutedEventArgs e)
        {
            _presenter.Logout();
        }

        private void SetWelcomeLabel(User user)
        {
            WelcomeUserLabel.Dispatch((l) => l.Content = string.IsNullOrEmpty(user.UserNsId) ? string.Empty : user.WelcomeMessage);
            if (user.Info == null) return;
            var buddyIconUri = new Uri(user.Info.BuddyIconUrl, UriKind.Absolute);
            buddyIcon.Dispatch((i) => i.Source = new BitmapImage(buddyIconUri));
        }

        private void ContinueButtonClick(object sender, RoutedEventArgs e)
        {
            var browserWindow = new BrowserWindow(User);
            browserWindow.Show();
            Close();
        }

        public void ShowSpinner(bool show)
        {
            var visibility = show ? Visibility.Visible : Visibility.Collapsed;
            Spinner.Dispatch((s) => s.Visibility = visibility);
        }
    }
}