using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FloydPink.Flickr.Downloadr
{
    public static class AppConstants
    {
        public static readonly string FlickrServicesUrlFormat = "http://api.flickr.com/services/rest/?method={0}&api_key=33fe2dc1389339c4e9cd77e9a90ebabf&format=json&nojsoncallback=1{1}";
        public static readonly string FlickrDictionaryContentKey = "_content";
    }
}
