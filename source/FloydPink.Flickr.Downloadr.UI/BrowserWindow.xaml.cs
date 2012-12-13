﻿using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
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

            _presenter = Bootstrapper.GetPresenter<IBrowserView, BrowserPresenter>(this);
            _presenter.InitializePhotoset();
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

        private void SelectAlreadySelectedPhotos()
        {
            if (SelectedPhotos.Count <= 0) return;
            foreach (var photo in Photos.Where(photo => SelectedPhotos.ContainsKey(photo.Id)))
            {
                ((ListBoxItem)PhotoList.ItemContainerGenerator.ContainerFromItem(photo)).IsSelected = true;
            }
        }

        public IDictionary<string, Photo> SelectedPhotos { get; set; }
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

        public void ShowSpinner(bool show)
        {
            var visibility = show ? Visibility.Visible : Visibility.Collapsed;
            Spinner.Dispatch((s) => s.Visibility = visibility);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow { User = User };
            loginWindow.Show();
            Close();
        }

        private void TogglePhotosButtonClick(object sender, RoutedEventArgs e)
        {
            _presenter.InitializePhotoset();
            LoseFocus((UIElement)sender);
        }

        private void FirstPageButtonClick(object sender, RoutedEventArgs e)
        {
            _presenter.GetFirstPagePhotos();
            LoseFocus((UIElement)sender);
        }

        private void PreviousPageButtonClick(object sender, RoutedEventArgs e)
        {
            _presenter.GetPreviousPagePhotos();
            LoseFocus((UIElement)sender);
        }

        private void NextPageButtonClick(object sender, RoutedEventArgs e)
        {
            _presenter.GetNextPagePhotos();
            LoseFocus((UIElement)sender);
        }

        private void LastPageButtonClick(object sender, RoutedEventArgs e)
        {
            _presenter.GetLastPagePhotos();
            LoseFocus((UIElement)sender);
        }

        private void DownloadSelectedButtonClick(object sender, RoutedEventArgs e)
        {
            _presenter.DownloadSelected();
            LoseFocus((UIElement)sender);
        }

        private void DownloadAllButtonClick(object sender, RoutedEventArgs e)
        {
            _presenter.DownloadAll();
            LoseFocus((UIElement)sender);
        }

        private void LoseFocus(UIElement element)
        {
            var scope = FocusManager.GetFocusScope(element);
            FocusManager.SetFocusedElement(scope, null);
            Keyboard.ClearFocus();
            FocusManager.SetFocusedElement(scope, PhotoList);
        }

    }
}