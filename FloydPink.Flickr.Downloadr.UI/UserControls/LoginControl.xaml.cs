using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FloydPink.Flickr.Downloadr.Views;
using FloydPink.Flickr.Downloadr.Presenters;
using FloydPink.Flickr.Downloadr.Bootstrap;

namespace FloydPink.Flickr.Downloadr.UI.UserControls
{
    /// <summary>
    /// Interaction logic for LoginControl.xaml
    /// </summary>
    public partial class LoginControl : UserControl, ILoginView
    {
        private LoginPresenter _presenter;

        public string UserName { get; set; }

        public LoginControl()
        {
            InitializeComponent();
            _presenter = Loader.GetPresenter<ILoginView, LoginPresenter>(this);
            _presenter.InitializeScreen();
        }

        public void ShowLoggedInControl()
        {
            loggedInCanvas.Visibility = System.Windows.Visibility.Visible;
            loggedOutCanvas.Visibility = System.Windows.Visibility.Hidden;
        }

        public void ShowLoggedOutControl()
        {
            loggedInCanvas.Visibility = System.Windows.Visibility.Hidden;
            loggedOutCanvas.Visibility = System.Windows.Visibility.Visible;
        }

        public void OpenAuthorizationUrl(string requestAuthUrl)
        {
            MessageBox.Show("You're going to be blah blah\r\n" + requestAuthUrl);
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            _presenter.LoginButtonClick();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            _presenter.CompleteLoginProcess(textBox1.Text.Trim());
        }

    }
}
