using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FloydPink.Flickr.Downloadr.Extensions
{
    public static class JsonDictionaryExtensions
    {
        public static object GetValueFromDictionary(this Dictionary<string, object> dictionary, string key, string subKey = AppConstants.FlickrDictionaryContentKey)
        {
            return ((Dictionary<string, object>)dictionary[key])[subKey];
        }
    }
}
