using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web.Helpers;
using System.Web.Script.Serialization;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth.ChannelElements;
using FloydPink.Flickr.Downloadr.OAuth;
using System.IO;
using FloydPink.Flickr.Downloadr.Listener;

namespace FloydPink.Flickr.Downloadr.Services
{
    public class FlickrServices
    {
        private Dictionary<string, string> defaultParameters = new Dictionary<string, string>() 
        { 
            { "nojsoncallback", "1" },
            { "format", "json" }
        };

        public OAuthManager OAuthManager { get; set; }

        public FlickrServices()
        {
            OAuthManager = new OAuthManager(new HttpListenerManager());
        }

        internal static string BuildServiceUrl(string methodName, Dictionary<string, string> parameters)
        {
            var parameterString = string.Join(string.Empty, parameters.Select(p => string.Format("&{0}={1}", p.Key, p.Value)));
            return string.Format(AppConstants.FlickrServicesUrlFormat, methodName, parameterString);
        }

        internal dynamic makeAnonymousRequest(string jsonServiceUrl)
        {
            using (var client = new WebClient())
            {
                var json = client.DownloadString(jsonServiceUrl);
                return (new JavaScriptSerializer()).Deserialize<dynamic>(json);
            }
        }

        internal dynamic makeAuthenticatedRequest(string methodName, IDictionary<string, string> parameters)
        {
            var allParameters = new Dictionary<string, string>(parameters);
            foreach (var kvp in defaultParameters)
                allParameters.Add(kvp.Key, kvp.Value);
            allParameters.Add("method", methodName);

            var request = OAuthManager.PrepareAuthorizedRequest(allParameters);
            var response = (HttpWebResponse)request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                return (new JavaScriptSerializer()).Deserialize<dynamic>(reader.ReadToEnd());
            }
        }

    }
}
