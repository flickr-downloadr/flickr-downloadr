using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using FloydPink.Flickr.Downloadr.Logic.Extensions;
using FloydPink.Flickr.Downloadr.Logic.Interfaces;
using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Model.Constants;
using FloydPink.Flickr.Downloadr.Model.Enums;
using FloydPink.Flickr.Downloadr.OAuth;
using log4net;

namespace FloydPink.Flickr.Downloadr.Logic
{
    public class BrowserLogic : IBrowserLogic
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (BrowserLogic));
        private readonly int _defaultPerPageCount;
        private readonly IDownloadLogic _downloadLogic;
        private readonly IOAuthManager _oAuthManager;

        public BrowserLogic(IOAuthManager oAuthManager, IDownloadLogic downloadLogic, int defaultPerPageCount)
        {
            _oAuthManager = oAuthManager;
            _defaultPerPageCount = defaultPerPageCount;
            _downloadLogic = downloadLogic;
        }

        #region IBrowserLogic Members

        public async Task<PhotosResponse> GetAllPhotosAsync(User user, int page = 1)
        {
            Log.Debug("In GetAllPhotosAsync Method.");

            return await GetPhotosAsync(user, page, Methods.PeopleGetPhotos);
        }

        public async Task<PhotosResponse> GetPublicPhotosAsync(User user, int page = 1)
        {
            Log.Debug("In GetPublicPhotosAsync Method.");

            return await GetPhotosAsync(user, page, Methods.PeopleGetPublicPhotos);
        }

        public async Task Download(IEnumerable<Photo> photos)
        {
            Log.Debug("In Download Method.");

            await _downloadLogic.Download(photos);
        }

        private async Task<PhotosResponse> GetPhotosAsync(User user, int page, string methodName)
        {
            Log.Debug("Entering GetPhotosAsync Method.");

            var extraParams = new Dictionary<string, string>
                                  {
                                      {ParameterNames.UserId, user.UserNsId},
                                      {ParameterNames.SafeSearch, SafeSearch.Safe},
                                      {
                                          ParameterNames.PerPage,
                                          _defaultPerPageCount.ToString(CultureInfo.InvariantCulture)
                                      },
                                      {ParameterNames.Page, page.ToString(CultureInfo.InvariantCulture)}
                                  };

            var photosResponse = (Dictionary<string, object>)
                                 await _oAuthManager.MakeAuthenticatedRequestAsync(methodName, extraParams);

            Log.Debug("Leaving GetPhotosAsync Method.");

            return photosResponse.GetPhotosResponseFromDictionary();
        }

        #endregion
    }
}