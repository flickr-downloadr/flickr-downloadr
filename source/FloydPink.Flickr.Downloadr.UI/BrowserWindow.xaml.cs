using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FloydPink.Flickr.Downloadr.Bootstrap;
using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Model.Enums;
using FloydPink.Flickr.Downloadr.Model.Extensions;
using FloydPink.Flickr.Downloadr.Presentation;
using FloydPink.Flickr.Downloadr.Presentation.Views;
using FloydPink.Flickr.Downloadr.UI.Extensions;

namespace FloydPink.Flickr.Downloadr.UI
{
    /// <summary>
    ///     Interaction logic for BrowserWindow.xaml
    /// </summary>
    public partial class BrowserWindow : Window, IBrowserView, INotifyPropertyChanged
    {
        private readonly BrowserPresenter _presenter;
        private bool _doNotSyncSelectedItems;
        private string _page;
        private string _pages;
        private string _perPage;
        private ObservableCollection<Photo> _photos;
        private string _total;

        public BrowserWindow(User user)
        {
            InitializeComponent();
            User = user;
            SelectedPhotos = new Dictionary<string, Dictionary<string, Photo>>();

            PhotoList.SelectionChanged += (sender, args) =>
                                              {
                                                  if (_doNotSyncSelectedItems) return;
                                                  SelectedPhotos[Page] = PhotoList.SelectedItems.Cast<Photo>().
                                                                                   ToDictionary(p => p.Id, p => p);
                                                  PropertyChanged.Notify(() => SelectedPhotosExist);
                                                  PropertyChanged.Notify(() => SelectedPhotosCountText);
                                              };

            SpinnerInner.SpinnerCanceled += (sender, args) => _presenter.CancelDownload();

            _presenter = Bootstrapper.GetPresenter<IBrowserView, BrowserPresenter>(this);
            _presenter.InitializePhotoset();
        }

        public int SelectedPhotosCount
        {
            get { return SelectedPhotos.Values.SelectMany(d => d.Values).Count(); }
        }

        public string SelectedPhotosCountText
        {
            get
            {
                var selectionCount = SelectedPhotosExist ? SelectedPhotosCount.ToString(CultureInfo.InvariantCulture) : string.Empty;
                return string.IsNullOrEmpty(selectionCount) ? "Selection" : string.Format("Selection ({0})", selectionCount);
            }
        }

        public bool SelectedPhotosExist
        {
            get { return SelectedPhotosCount != 0; }
        }

        public string FirstPhoto
        {
            get
            {
                return
                    (((Convert.ToInt32(Page) - 1) * Convert.ToInt32(PerPage)) + 1).ToString(CultureInfo.InvariantCulture);
            }
        }

        public string LastPhoto
        {
            get
            {
                int maxLast = Convert.ToInt32(Page) * Convert.ToInt32(PerPage);
                return maxLast > Convert.ToInt32(Total) ? Total : maxLast.ToString(CultureInfo.InvariantCulture);
            }
        }

        public User User { get; set; }

        public ObservableCollection<Photo> Photos
        {
            get { return _photos; }
            set
            {
                _photos = value;
                _doNotSyncSelectedItems = true;
                PhotoList.DataContext = Photos;
                SelectAlreadySelectedPhotos();
                _doNotSyncSelectedItems = false;
            }
        }

        public IDictionary<string, Dictionary<string, Photo>> SelectedPhotos { get; set; }

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
                PropertyChanged.Notify(() => FirstPhoto);
                PropertyChanged.Notify(() => LastPhoto);
            }
        }

        public void ShowSpinner(bool show)
        {
            Visibility visibility = show ? Visibility.Visible : Visibility.Collapsed;
            Spinner.Dispatch((s) => s.Visibility = visibility);
        }

        public void UpdateProgress(string percentDone, string operationText, bool cancellable)
        {
            SpinnerInner.Dispatch(sc => sc.Cancellable = cancellable);
            SpinnerInner.Dispatch(sc => sc.OperationText = operationText);
            SpinnerInner.Dispatch(sc => sc.PercentDone = percentDone);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void SelectAlreadySelectedPhotos()
        {
            if (!SelectedPhotos.ContainsKey(Page) || SelectedPhotos[Page].Count <= 0) return;
            List<Photo> photos = Photos.Where(photo => SelectedPhotos[Page].ContainsKey(photo.Id)).ToList();
            foreach (Photo photo in photos)
            {
                ((ListBoxItem)PhotoList.ItemContainerGenerator.ContainerFromItem(photo)).IsSelected = true;
            }
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow { User = User };
            loginWindow.Show();
            Close();
        }

        private async void TogglePhotosButtonClick(object sender, RoutedEventArgs e)
        {
            LoseFocus((UIElement)sender);
            await _presenter.InitializePhotoset();
        }

        private async void FirstPageButtonClick(object sender, RoutedEventArgs e)
        {
            LoseFocus((UIElement)sender);
            await _presenter.NavigateTo(PhotoPage.First);
        }

        private async void PreviousPageButtonClick(object sender, RoutedEventArgs e)
        {
            LoseFocus((UIElement)sender);
            await _presenter.NavigateTo(PhotoPage.Previous);
        }

        private async void NextPageButtonClick(object sender, RoutedEventArgs e)
        {
            LoseFocus((UIElement)sender);
            await _presenter.NavigateTo(PhotoPage.Next);
        }

        private async void LastPageButtonClick(object sender, RoutedEventArgs e)
        {
            LoseFocus((UIElement)sender);
            await _presenter.NavigateTo(PhotoPage.Last);
        }

        private async void DownloadSelectionButtonClick(object sender, RoutedEventArgs e)
        {
            LoseFocus((UIElement)sender);
            await _presenter.DownloadSelection();
        }

        private async void DownloadThisPageButtonClick(object sender, RoutedEventArgs e)
        {
            LoseFocus((UIElement)sender);
            await _presenter.DownloadThisPage();
        }

        private async void DownloadAllPagesButtonClick(object sender, RoutedEventArgs e)
        {
            LoseFocus((UIElement)sender);
            await _presenter.DownloadAllPages();
        }

        private void LoseFocus(UIElement element)
        {
            // http://stackoverflow.com/a/6031393/218882
            DependencyObject scope = FocusManager.GetFocusScope(element);
            FocusManager.SetFocusedElement(scope, null);
            Keyboard.ClearFocus();
            FocusManager.SetFocusedElement(scope, PhotoList);
        }
    }
}