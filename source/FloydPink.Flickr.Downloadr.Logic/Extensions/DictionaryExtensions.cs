using System;
using System.Collections.Generic;
using System.Linq;
using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Model.Constants;


namespace FloydPink.Flickr.Downloadr.Logic.Extensions
{
    public static class DictionaryExtensions
    {

        public static object GetValueFromDictionary(this Dictionary<string, object> dictionary, string key,
                                                    string subKey = AppConstants.FlickrDictionaryContentKey)
        {
            if (dictionary.ContainsKey(key))
            {
                var subDictionary = (Dictionary<string, object>)dictionary[key];
                return subDictionary.ContainsKey(subKey) ? subDictionary[subKey] : null;
            }
            return null;
        }

        public static PhotosResponse GetPhotosResponseFromDictionary(this Dictionary<string, object> dictionary)
        {
            var photos = new List<Photo>();
            IEnumerable<Dictionary<string, object>> photoDictionary =
                ((IEnumerable<object>)dictionary.GetValueFromDictionary("photos", "photo")).
                    Cast<Dictionary<string, object>>();

            photos.AddRange(photoDictionary.Select(BuildPhoto));

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
                             Convert.ToBoolean(dictionary["isfamily"]),
                             dictionary.GetValueFromDictionary("description").ToString(),
                             dictionary["tags"].ToString(),
                             dictionary["originalsecret"].ToString(),
                             dictionary["originalformat"].ToString());
        }
    }
}