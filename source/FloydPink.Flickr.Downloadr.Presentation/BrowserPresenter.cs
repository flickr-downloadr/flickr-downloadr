using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using FloydPink.Flickr.Downloadr.Logic.Interfaces;
using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Model.Enums;
using FloydPink.Flickr.Downloadr.Presentation.Views;


namespace FloydPink.Flickr.Downloadr.Presentation
{
    public class BrowserPresenter : PresenterBase
    {
        private readonly IBrowserLogic _logic;
        private readonly IBrowserView _view;
        private readonly Progress<int> _progress = new Progress<int>();
        private CancellationTokenSource _cancellationTokenSource;

        public BrowserPresenter(IBrowserLogic logic, IBrowserView view)
        {
            _logic = logic;
            _view = view;
            _progress.ProgressChanged += (sender, progress) =>
                _view.UpdateProgress(string.Format("{0}%", progress.ToString(CultureInfo.InvariantCulture)));
        }

        public async Task InitializePhotoset()
        {
            _view.SelectedPhotos.Clear();
            await GetAndSetPhotos(1);
        }

        public async Task NavigateTo(PhotoPage page)
        {
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
        }

        public async Task DownloadSelection()
        {
            _view.ShowSpinner(true);
            _cancellationTokenSource = new CancellationTokenSource();
            await _logic.Download(_view.SelectedPhotos.Values, _cancellationTokenSource.Token, _progress);
            _view.ShowSpinner(false);
        }

        public async Task DownloadThisPage()
        {
            _view.ShowSpinner(true);
            _cancellationTokenSource = new CancellationTokenSource();
            await _logic.Download(_view.Photos, _cancellationTokenSource.Token, _progress);
            _view.ShowSpinner(false);
        }

        public async Task DownloadAllPages()
        {
            _view.ShowSpinner(true);
            //TODO: Implement this!
            _cancellationTokenSource = new CancellationTokenSource();
            await _logic.Download(_view.Photos, _cancellationTokenSource.Token, _progress);
            _view.ShowSpinner(false);
        }

        private async Task GetAndSetPhotos(int page)
        {
            _view.ShowSpinner(true);
            SetPhotoResponse(
                await
                (_view.ShowAllPhotos
                     ? _logic.GetAllPhotosAsync(_view.User, page)
                     : _logic.GetPublicPhotosAsync(_view.User, page)));
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

        public void CancelDownload()
        {
            if (!_cancellationTokenSource.IsCancellationRequested)
                _cancellationTokenSource.Cancel();
        }
    }
}