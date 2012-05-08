using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FloydPink.Flickr.Downloadr.Extensions
{
    public static class FlickrExtensions
    {
        public static object GetValueFromDictionary(this Dictionary<string, object> dictionary, string key)
        {
            return ((Dictionary<string, object>)dictionary[key])[AppConstants.FlickrDictionaryContentKey];
        }
    }
}
