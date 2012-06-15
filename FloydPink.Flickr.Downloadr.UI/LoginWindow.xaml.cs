using System;
using System.Windows;
using System.Windows.Media.Imaging;
using FloydPink.Flickr.Downloadr.Bootstrap;
using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Presenters;
using FloydPink.Flickr.Downloadr.UI.Extensions;
using FloydPink.Flickr.Downloadr.Views;

namespace FloydPink.Flickr.Downloadr.UI
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window, ILoginView
    {
        private readonly LoginPresenter _presenter;
        private User _user;

        public LoginWindow()
        {
            InitializeComponent();
            User = new User();

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
            loggedInCanvas.Dispatch((c) => c.Visibility = Visibility.Visible);
            loggedOutCanvas.Dispatch((c) => c.Visibility = Visibility.Collapsed);
        }

        public void ShowLoggedOutControl()
        {
            loggedOutCanvas.Dispatch((c) => c.Visibility = Visibility.Visible);
            loggedInCanvas.Dispatch((c) => c.Visibility = Visibility.Collapsed);
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
            string userNameString = string.IsNullOrEmpty(user.Name)
                                        ? (string.IsNullOrEmpty(user.Username) ? string.Empty : user.Username)
                                        : user.Name;
            string welcomeMessage = string.IsNullOrEmpty(userNameString)
                                        ? string.Empty
                                        : string.Format("Welcome, {0}!", userNameString);
            welcomeUserLabel.Dispatch(
                (l) => l.Content = string.IsNullOrEmpty(user.UserNsId) ? string.Empty : welcomeMessage);
            if (user.Info != null)
            {
                var buddyIconUri = new Uri(user.Info.BuddyIconUrl, UriKind.Absolute);
                buddyIcon.Dispatch((i) => i.Source = new BitmapImage(buddyIconUri));
            }
        }
    }
}