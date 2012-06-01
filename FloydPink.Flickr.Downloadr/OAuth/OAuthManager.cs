using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using FloydPink.Flickr.Downloadr.Listener;
using FloydPink.Flickr.Downloadr.Model;

namespace FloydPink.Flickr.Downloadr.OAuth
{
    public class OAuthManager : IOAuthManager
    {
        private IHttpListenerManager _listenerManager;
        private DesktopConsumer _consumer;
        private MessageReceivingEndpoint _serviceEndPoint;

        private Dictionary<string, string> defaultParameters = new Dictionary<string, string>() 
        { 
            { "nojsoncallback", "1" },
            { "format", "json" }
        };

        private string RequestToken = string.Empty;

        public string AccessToken { get; set; }

        public event EventHandler<AuthenticatedEventArgs> Authenticated;

        public OAuthManager(IHttpListenerManager listenerManager, DesktopConsumer consumer, MessageReceivingEndpoint serviceEndPoint)
        {
            _listenerManager = listenerManager;
            _consumer = consumer;
            _serviceEndPoint = serviceEndPoint;
        }

        public string BeginAuthorization()
        {
            _listenerManager.RequestReceived += new EventHandler<HttpListenerCallbackEventArgs>(callbackManager_OnRequestReceived);
            _listenerManager.ResponseString = AppConstants.AuthenticatedMessage;
            _listenerManager.SetupCallback();
            var requestArgs = new Dictionary<string, string>() { 
                { "oauth_callback", _listenerManager.ListenerAddress }
            };
            var redirectArgs = new Dictionary<string, string>() { 
                { "perms", "read" } 
            };
            return _consumer.RequestUserAuthorization(requestArgs, redirectArgs, out this.RequestToken).AbsoluteUri;
        }

        public HttpWebRequest PrepareAuthorizedRequest(IDictionary<string, string> parameters)
        {
            return _consumer.PrepareAuthorizedRequest(_serviceEndPoint, this.AccessToken, parameters);
        }

        public dynamic MakeAuthenticatedRequest(string methodName, IDictionary<string, string> parameters = null)
        {
            parameters = parameters ?? new Dictionary<string, string>();
            var allParameters = new Dictionary<string, string>(parameters);
            foreach (var kvp in defaultParameters)
                allParameters.Add(kvp.Key, kvp.Value);
            allParameters.Add("method", methodName);

            var request = PrepareAuthorizedRequest(allParameters);
            var response = (HttpWebResponse)request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                return (new JavaScriptSerializer()).Deserialize<dynamic>(reader.ReadToEnd());
            }
        }

        private string CompleteAuthorization(string verifier)
        {
            var response = _consumer.ProcessUserAuthorization(this.RequestToken, verifier);
            this.AccessToken = response.AccessToken;

            var extraData = response.ExtraData;
            var authenticatedUser = new User(extraData["fullname"], extraData["username"], extraData["user_nsid"]);
            Authenticated(this, new AuthenticatedEventArgs(authenticatedUser));

            return response.AccessToken;
        }

        private void callbackManager_OnRequestReceived(object sender, HttpListenerCallbackEventArgs e)
        {
            var token = e.QueryStrings["oauth_token"];
            var verifier = e.QueryStrings["oauth_verifier"];
            if (token == this.RequestToken)
            {
                CompleteAuthorization(verifier);
            }
        }
    }
}
