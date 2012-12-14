﻿using System;
using System.Globalization;
using System.Threading.Tasks;
using FloydPink.Flickr.Downloadr.Logic.Interfaces;
using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Model.Enums;
using FloydPink.Flickr.Downloadr.Presentation.Views;
using System.Collections.ObjectModel;

namespace FloydPink.Flickr.Downloadr.Presentation
{
    public class BrowserPresenter : PresenterBase
    {
        private readonly IBrowserLogic _logic;
        private readonly IBrowserView _view;

        public BrowserPresenter(IBrowserLogic logic, IBrowserView view)
        {
            _logic = logic;
            _view = view;
        }

        public async Task InitializePhotoset()
        {
            _view.SelectedPhotos.Clear();
            await GetAndSetPhotos(1);
        }

        public async Task NavigateTo(PhotoPage page)
        {
            var targetPage = 0;
            var currentPage = Convert.ToInt32(_view.Page);
            var totalPages = Convert.ToInt32(_view.Pages);
            switch (page)
            {
                case PhotoPage.First:
                    if (currentPage != 1) targetPage = 1;
                    break;
                case PhotoPage.Previous:
                    if (currentPage != 1) targetPage = currentPage - 1;
                    break;
                case PhotoPage.Next:
                    if (currentPage != totalPages) targetPage = currentPage + 1;
                    break;
                case PhotoPage.Last:
                    if (currentPage != totalPages) targetPage = totalPages;
                    break;
            }
            await GetAndSetPhotos(targetPage);
        }

        public async Task DownloadSelection()
        {
            _view.ShowSpinner(true);
            await _logic.Download(_view.SelectedPhotos.Values);
            _view.ShowSpinner(false);
        }

        public async Task DownloadThisPage()
        {
            _view.ShowSpinner(true);
            await _logic.Download(_view.Photos);
            _view.ShowSpinner(false);
        }

        public async Task DownloadAllPages()
        {
            _view.ShowSpinner(true);
            //TODO: Implement this!
            await _logic.Download(_view.Photos);
            _view.ShowSpinner(false);
        }

        private async Task GetAndSetPhotos(int page)
        {
            _view.ShowSpinner(true);

            SetPhotoResponse(await (_view.ShowAllPhotos ? _logic.GetAllPhotosAsync(_view.User, page) : _logic.GetPublicPhotosAsync(_view.User, page)));

            _view.ShowSpinner(false);
        }

        private void SetPhotoResponse(PhotosResponse photosResponse)
        {
            _view.Photos = new ObservableCollection<Photo>(photosResponse.Photos);
            _view.Page = photosResponse.Page.ToString(CultureInfo.InvariantCulture);
            _view.Pages = photosResponse.Pages.ToString(CultureInfo.InvariantCulture);
            _view.PerPage = photosResponse.PerPage.ToString(CultureInfo.InvariantCulture);
            _view.Total = photosResponse.Total.ToString(CultureInfo.InvariantCulture);
        }
    }
}