using System;
using System.Windows;
using System.Windows.Controls;
using FloydPink.Flickr.Downloadr.Bootstrap;
using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Presenters;
using FloydPink.Flickr.Downloadr.Views;

namespace FloydPink.Flickr.Downloadr.UI.UserControls
{
    /// <summary>
    /// Interaction logic for LoginControl.xaml
    /// </summary>
    public partial class LoginControl : UserControl, ILoginView
    {
        private LoginPresenter _presenter;

        public User User { get; set; }

        public LoginControl()
        {
            InitializeComponent();
            DataContext = User;
            _presenter = Loader.GetPresenter<ILoginView, LoginPresenter>(this);
            _presenter.InitializeScreen();
        }

        public void ShowLoggedInControl()
        {
            loggedInCanvas.Visibility = System.Windows.Visibility.Visible;
            loggedOutCanvas.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void ShowLoggedOutControl()
        {
            loggedOutCanvas.Visibility = System.Windows.Visibility.Visible;
            loggedInCanvas.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            _presenter.LoginButtonClick();
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            _presenter.LogoutButtonClick();
        }

    }
}
