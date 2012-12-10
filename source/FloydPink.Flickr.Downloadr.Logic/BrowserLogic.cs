using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using FloydPink.Flickr.Downloadr.Logic.Extensions;
using FloydPink.Flickr.Downloadr.Logic.Interfaces;
using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Model.Constants;
using FloydPink.Flickr.Downloadr.Model.Enums;
using FloydPink.Flickr.Downloadr.OAuth;

namespace FloydPink.Flickr.Downloadr.Logic
{
    public class BrowserLogic : IBrowserLogic
    {
        private readonly int _defaultPerPageCount;
        private readonly IOAuthManager _oAuthManager;

        public BrowserLogic(IOAuthManager oAuthManager, int defaultPerPageCount)
        {
            _oAuthManager = oAuthManager;
            _defaultPerPageCount = defaultPerPageCount;
        }

        #region IBrowserLogic Members

        public async Task<PhotosResponse> GetPublicPhotosAsync(User user, int page = 1)
        {
            var publicPhotosResponse = (Dictionary<string, object>)
                                       await _oAuthManager.MakeAuthenticatedRequestAsync(Methods.PeopleGetPublicPhotos,
                                                                                         FormatExtraParams(user, page));
            return publicPhotosResponse.GetPhotosResponseFromDictionary();
        }

        private Dictionary<string, string> FormatExtraParams(User user, int page)
        {
            return new Dictionary<string, string>
                       {
                           {ParameterNames.UserId, user.UserNsId},
                           {ParameterNames.SafeSearch, SafeSearch.Safe},
                           {
                               ParameterNames.PerPage,
                               _defaultPerPageCount.ToString(CultureInfo.InvariantCulture)
                           },
                           {ParameterNames.Page, page.ToString(CultureInfo.InvariantCulture)}
                       };
        }

        #endregion
    }
}