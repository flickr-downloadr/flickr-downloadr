using System;
using System.Globalization;
using System.Threading.Tasks;
using FloydPink.Flickr.Downloadr.Logic.Interfaces;
using FloydPink.Flickr.Downloadr.Model;
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

        public async void InitializePhotoset()
        {
            _view.SelectedPhotos.Clear();
            await GetAndSetPhotos(1);
        }

        public async void DownloadAll()
        {
            _view.ShowSpinner(true);
            await _logic.Download(_view.Photos);
            _view.ShowSpinner(false);
        }

        public async void DownloadSelected()
        {
            _view.ShowSpinner(true);
            await _logic.Download(_view.SelectedPhotos.Values);
            _view.ShowSpinner(false);
        }

        public async void GetFirstPagePhotos()
        {
            var page = Convert.ToInt32(_view.Page);
            if (page != 1)
            {
                await GetAndSetPhotos(1);
            }
        }

        public async void GetPreviousPagePhotos()
        {
            var page = Convert.ToInt32(_view.Page);
            if (page != 1)
            {
                await GetAndSetPhotos(page - 1);
            }
        }

        public async void GetNextPagePhotos()
        {
            var page = Convert.ToInt32(_view.Page);
            var pages = Convert.ToInt32(_view.Pages);
            if (page != pages)
            {
                await GetAndSetPhotos(page + 1);
            }
        }

        public async void GetLastPagePhotos()
        {
            var page = Convert.ToInt32(_view.Page);
            var pages = Convert.ToInt32(_view.Pages);
            if (page != pages)
            {
                await GetAndSetPhotos(pages);
            }
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