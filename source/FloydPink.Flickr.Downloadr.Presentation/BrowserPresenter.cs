using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using FloydPink.Flickr.Downloadr.Logic.Interfaces;
using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Model.Enums;
using FloydPink.Flickr.Downloadr.Presentation.Views;
using log4net;

namespace FloydPink.Flickr.Downloadr.Presentation
{
    public class BrowserPresenter : PresenterBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (BrowserPresenter));
        private readonly IBrowserLogic _logic;
        private readonly IBrowserView _view;

        public BrowserPresenter(IBrowserLogic logic, IBrowserView view)
        {
            _logic = logic;
            _view = view;
        }

        public async Task InitializePhotoset()
        {
            Log.Debug("Entering InitializePhotoset Method.");

            _view.SelectedPhotos.Clear();
            await GetAndSetPhotos(1);

            Log.Debug("Leaving InitializePhotoset Method.");
        }

        public async Task NavigateTo(PhotoPage page)
        {
            Log.Debug("Entering NavigateTo Method. Parameter: page = " + page);

            int targetPage = 0;
            int currentPage = Convert.ToInt32(_view.Page);
            int totalPages = Convert.ToInt32(_view.Pages);
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
            if (targetPage != 0) await GetAndSetPhotos(targetPage);

            Log.Debug("Leaving NavigateTo Method.");
        }

        public async Task DownloadSelection()
        {
            Log.Debug("Entering DownloadSelection Method.");

            _view.ShowSpinner(true);
            await _logic.Download(_view.SelectedPhotos.Values);
            _view.ShowSpinner(false);

            Log.Debug("Leaving DownloadSelection Method.");
        }

        public async Task DownloadThisPage()
        {
            Log.Debug("Entering DownloadThisPage Method.");

            _view.ShowSpinner(true);
            await _logic.Download(_view.Photos);
            _view.ShowSpinner(false);

            Log.Debug("Leaving DownloadThisPage Method.");
        }

        public async Task DownloadAllPages()
        {
            Log.Debug("Entering DownloadAllPages Method.");

            _view.ShowSpinner(true);
            //TODO: Implement this!
            await _logic.Download(_view.Photos);
            _view.ShowSpinner(false);

            Log.Debug("Leaving DownloadAllPages Method.");
        }

        private async Task GetAndSetPhotos(int page)
        {
            Log.Debug("Entering GetAndSetPhotos Method.");

            _view.ShowSpinner(true);
            SetPhotoResponse(
                await
                (_view.ShowAllPhotos
                     ? _logic.GetAllPhotosAsync(_view.User, page)
                     : _logic.GetPublicPhotosAsync(_view.User, page)));
            _view.ShowSpinner(false);

            Log.Debug("Leaving GetAndSetPhotos Method.");
        }

        private void SetPhotoResponse(PhotosResponse photosResponse)
        {
            Log.Debug("Entering SetPhotoResponse Method.");

            _view.Photos = new ObservableCollection<Photo>(photosResponse.Photos);
            _view.Page = photosResponse.Page.ToString(CultureInfo.InvariantCulture);
            _view.Pages = photosResponse.Pages.ToString(CultureInfo.InvariantCulture);
            _view.PerPage = photosResponse.PerPage.ToString(CultureInfo.InvariantCulture);
            _view.Total = photosResponse.Total.ToString(CultureInfo.InvariantCulture);

            Log.Debug("Leaving SetPhotoResponse Method.");
        }
    }
}