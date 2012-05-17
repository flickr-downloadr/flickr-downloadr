using System.Windows;
using FloydPink.Flickr.Downloadr.Bootstrap;
using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Presenters;
using FloydPink.Flickr.Downloadr.UI.Extensions;
using FloydPink.Flickr.Downloadr.Views;

namespace FloydPink.Flickr.Downloadr.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ILoginView
    {
        private User _user;
        private LoginPresenter _presenter;

        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                setWelcomeLabel(value);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            User = new User();

            _presenter = Loader.GetPresenter<ILoginView, LoginPresenter>(this);
            _presenter.InitializeScreen();
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

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            _presenter.Login();
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            _presenter.Logout();
        }

        private void setWelcomeLabel(User user)
        {
            var userNameString = string.IsNullOrEmpty(user.Name) ?
                (string.IsNullOrEmpty(user.Username) ? string.Empty : user.Username) :
                user.Name;
            var welcomeMessage = string.IsNullOrEmpty(userNameString) ? string.Empty : 
                string.Format("Welcome, {0}!", userNameString);
            welcomeUserLabel.Dispatch((l) => l.Content = string.IsNullOrEmpty(user.UserNSId) ? string.Empty : welcomeMessage);
        }

    }
}
