using System.Collections.Generic;
using System.Globalization;
using FloydPink.Flickr.Downloadr.Constants;
using FloydPink.Flickr.Downloadr.Enums;
using FloydPink.Flickr.Downloadr.Extensions;
using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.OAuth;

namespace FloydPink.Flickr.Downloadr.Logic
{
    public class BrowserLogic : IBrowserLogic
    {
        private readonly IOAuthManager _oAuthManager;
        private readonly int _defaultPerPageCount;

        public BrowserLogic(IOAuthManager oAuthManager, int defaultPerPageCount)
        {
            _oAuthManager = oAuthManager;
            _defaultPerPageCount = defaultPerPageCount;
        }

        public IEnumerable<Photo> GetPublicPhotos(User user, int page)
        {
            var extraParams = new Dictionary<string, string>
            { 
                { ParameterNames.UserId, user.UserNsId },
                { ParameterNames.SafeSearch, SafeSearch.Safe },
                { ParameterNames.PerPage, _defaultPerPageCount.ToString(CultureInfo.InvariantCulture) },
                { ParameterNames.Page, page.ToString(CultureInfo.InvariantCulture) }
            };
            var publicPhotosResponse = (Dictionary<string, object>)_oAuthManager.MakeAuthenticatedRequest(Methods.PeopleGetPublicPhotos, extraParams);
            return publicPhotosResponse.GetPhotosFromDictionary();
        }
    }
}