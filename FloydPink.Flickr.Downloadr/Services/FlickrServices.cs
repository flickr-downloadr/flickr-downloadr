using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web.Helpers;
using System.Web.Script.Serialization;

namespace FloydPink.Flickr.Downloadr.Services
{
    public class FlickrServices
    {
        public FlickrServices()
        {

        }

        internal static string BuildServiceUrl(string methodName, Dictionary<string, string> parameters)
        {
            var parameterString = string.Join(string.Empty, parameters.Select(p => string.Format("&{0}={1}", p.Key, p.Value)));
            return string.Format(AppConstants.FlickrServicesUrlFormat, methodName, parameterString);
        }

        internal static object GetValueFromDictionary(Dictionary<string, object> dictionary, string key)
        {
            return ((Dictionary<string, object>)dictionary[key])[AppConstants.FlickrDictionaryContentKey];
        }

        internal dynamic makeFlickrServiceCall(string jsonServiceUrl)
        {
            using (var client = new WebClient())
            {
                var json = client.DownloadString(jsonServiceUrl);
                return (new JavaScriptSerializer()).Deserialize<dynamic>(json);
            }
        }

    }
}
