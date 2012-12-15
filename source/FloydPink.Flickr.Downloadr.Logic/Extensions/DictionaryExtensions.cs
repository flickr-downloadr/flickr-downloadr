using System;
using System.Collections.Generic;
using System.Linq;
using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Model.Constants;
using log4net;

namespace FloydPink.Flickr.Downloadr.Logic.Extensions
{
    public static class DictionaryExtensions
    {

        private static readonly ILog Log = LogManager.GetLogger(typeof(DictionaryExtensions));

        public static object GetValueFromDictionary(this Dictionary<string, object> dictionary, string key,
                                                    string subKey = AppConstants.FlickrDictionaryContentKey)
        {
            if (dictionary.ContainsKey(key))
            {
                var subDictionary = (Dictionary<string, object>)dictionary[key];

                Log.Debug("Leaving GetValueFromDictionary Method.");
                return subDictionary.ContainsKey(subKey) ? subDictionary[subKey] : null;
            }
            return null;
        }

        public static PhotosResponse GetPhotosResponseFromDictionary(this Dictionary<string, object> dictionary)
        {
            Log.Debug("Entering GetPhotosResponseFromDictionary Method.");

            var photos = new List<Photo>();
            IEnumerable<Dictionary<string, object>> photoDictionary =
                ((IEnumerable<object>)dictionary.GetValueFromDictionary("photos", "photo")).
                    Cast<Dictionary<string, object>>();

            Log.Debug("About to call BuildPhoto method with " + photos.Count + " photos.");
            photos.AddRange(photoDictionary.Select(BuildPhoto));

            Log.Debug("Leaving GetPhotosResponseFromDictionary Method.");

            return new PhotosResponse(
                Convert.ToInt32(dictionary.GetValueFromDictionary("photos", "page")),
                Convert.ToInt32(dictionary.GetValueFromDictionary("photos", "pages")),
                Convert.ToInt32(dictionary.GetValueFromDictionary("photos", "perpage")),
                Convert.ToInt32(dictionary.GetValueFromDictionary("photos", "total")),
                photos);
        }

        private static Photo BuildPhoto(Dictionary<string, object> dictionary)
        {
            return new Photo(dictionary["id"].ToString(),
                             dictionary["owner"].ToString(),
                             dictionary["secret"].ToString(),
                             dictionary["server"].ToString(),
                             Convert.ToInt32(dictionary["farm"]),
                             dictionary["title"].ToString(),
                             Convert.ToBoolean(dictionary["ispublic"]),
                             Convert.ToBoolean(dictionary["isfriend"]),
                             Convert.ToBoolean(dictionary["isfamily"]));
        }
    }
}