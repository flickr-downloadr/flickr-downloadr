using System.Collections.ObjectModel;
using FloydPink.Flickr.Downloadr.Logic.Interfaces;
using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Presentation.Views;

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

        public async void InitializeScreen()
        {
            _view.ShowSpinner(true);
            var photosResponse = await _logic.GetAllPhotosAsync(_view.User);
            _view.Photos = new ObservableCollection<Photo>(photosResponse.Photos);
            _view.ShowSpinner(false);
        }

        public async void DownloadSelected()
        {
            _view.ShowSpinner(true);
            await _logic.Download(_view.Photos);
            _view.ShowSpinner(false);
        }

        public async void DownloadAll()
        {
            _view.ShowSpinner(true);
            await _logic.Download(_view.Photos);
            _view.ShowSpinner(false);
        }

        public void TogglePhotos()
        {
            //            var publicPhotosMode = _view.TogglePhotosButtonCaption == "All Photos";
            //            _view.TogglePhotosButtonCaption = publicPhotosMode ? "Only Public Photos" : "All Photos";
        }
    }
}