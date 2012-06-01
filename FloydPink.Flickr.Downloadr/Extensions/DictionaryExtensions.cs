using System.Collections.Generic;

namespace FloydPink.Flickr.Downloadr.Extensions
{
    public static class DictionaryExtensions
    {
        public static object GetValueFromDictionary(this Dictionary<string, object> dictionary, string key, string subKey = AppConstants.FlickrDictionaryContentKey)
        {
            if (dictionary.ContainsKey(key))
            {
                var subDictionary = (Dictionary<string, object>)dictionary[key];
                return subDictionary.ContainsKey(subKey) ? subDictionary[subKey] : null;
            }
            return null;
        }
    }
}
