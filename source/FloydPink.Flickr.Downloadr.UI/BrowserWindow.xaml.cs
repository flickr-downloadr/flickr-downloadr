using System.ComponentModel;
using FloydPink.Flickr.Downloadr.Bootstrap;
using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Model.Extensions;
using FloydPink.Flickr.Downloadr.Presentation;
using FloydPink.Flickr.Downloadr.Presentation.Views;
using FloydPink.Flickr.Downloadr.UI.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace FloydPink.Flickr.Downloadr.UI
{
    /// <summary>
    ///     Interaction logic for BrowserWindow.xaml
    /// </summary>
    public partial class BrowserWindow : Window, IBrowserView, INotifyPropertyChanged
    {
        private readonly BrowserPresenter _presenter;
        private ObservableCollection<Photo> _photos;
        private string _page;
        private string _pages;
        private string _perPage;
        private string _total;

        public BrowserWindow(User user)
        {
            InitializeComponent();
            User = user;
            SelectedPhotos = new List<Photo>();

            PhotoList.SelectionChanged += (sender, args) =>
                                              {
                                                  SelectedPhotos.Clear();
                                                  foreach (var selectedItem in PhotoList.SelectedItems)
                                                  {
                                                      SelectedPhotos.Add((Photo)selectedItem);
                                                  }
                                              };

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

        public IList<Photo> SelectedPhotos { get; set; }
        public bool ShowAllPhotos
        {
            get { return PublicAllToggleButton.IsChecked != null && (bool)PublicAllToggleButton.IsChecked; }
        }

        public string Page
        {
            get { return _page; }
            set
            {
                _page = value;
                PropertyChanged.Notify(() => Page);
            }
        }

        public string Pages
        {
            get { return _pages; }
            set
            {
                _pages = value;
                PropertyChanged.Notify(() => Pages);
            }
        }

        public string PerPage
        {
            get { return _perPage; }
            set
            {
                _perPage = value;
                PropertyChanged.Notify(() => PerPage);
            }
        }

        public string Total
        {
            get { return _total; }
            set
            {
                _total = value;
                PropertyChanged.Notify(() => Total);
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
            var loginWindow = new LoginWindow { User = User };
            loginWindow.Show();
            Close();
        }

        private void TogglePhotosButtonClick(object sender, RoutedEventArgs e)
        {
            _presenter.InitializeScreen();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void FirstPageButtonClick(object sender, RoutedEventArgs e)
        {
            _presenter.GetFirstPagePhotos();
        }

        private void PreviousPageButtonClick(object sender, RoutedEventArgs e)
        {
            _presenter.GetPreviousPagePhotos();
        }

        private void NextPageButtonClick(object sender, RoutedEventArgs e)
        {
            _presenter.GetNextPagePhotos();
        }

        private void LastPageButtonClick(object sender, RoutedEventArgs e)
        {
            _presenter.GetLastPagePhotos(); 
        }
    }
}