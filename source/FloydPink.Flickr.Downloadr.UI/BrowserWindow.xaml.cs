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
        private ObservableCollection<Photo> _photos;

        public BrowserWindow(User user)
        {
            InitializeComponent();
            User = user;
            SelectedPhotos = new Dictionary<string, Photo>();

            PhotoList.SelectionChanged += (sender, args) =>
                                              {
                                                  foreach (Photo photo in PhotoList.SelectedItems)
                                                  {
                                                      if (!SelectedPhotos.ContainsKey(photo.Id))
                                                      {
                                                          SelectedPhotos.Add(photo.Id, photo);
                                                      }
                                                  }
                                              };

            SpinnerInner.SpinnerCanceled += (sender, args) => _presenter.CancelDownload();

            _presenter = Bootstrapper.GetPresenter<IBrowserView, BrowserPresenter>(this);
            _presenter.InitializePhotoset();
        }

        public string FirstPhoto
        {
            get
            {
                return (((Convert.ToInt32(Page) - 1) * Convert.ToInt32(PerPage)) + 1).ToString(CultureInfo.InvariantCulture);
            }
        }

        public string LastPhoto
        {
            get
            {
                var maxLast = Convert.ToInt32(Page) * Convert.ToInt32(PerPage);
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
                PhotoList.DataContext = Photos;
                SelectAlreadySelectedPhotos();
            }
        }

        public IDictionary<string, Photo> SelectedPhotos { get; set; }

        public bool ShowAllPhotos
        {
            get { return PublicAllToggleButton.IsChecked != null && (bool)PublicAllToggleButton.IsChecked; }
        }

        public string Page { get; set; }

        public string Pages { get; set; }

        public string PerPage { get; set; }

        public string Total { get; set; }

        public void RaiseNotify()
        {
            PropertyChanged.Notify(() => Page);
            PropertyChanged.Notify(() => Pages);
            PropertyChanged.Notify(() => PerPage);
            PropertyChanged.Notify(() => Total);
            PropertyChanged.Notify(() => FirstPhoto);
            PropertyChanged.Notify(() => LastPhoto);
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
            if (SelectedPhotos.Count <= 0) return;
            foreach (Photo photo in Photos.Where(photo => SelectedPhotos.ContainsKey(photo.Id)))
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