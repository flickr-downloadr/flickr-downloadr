using System;
using System.Collections.Generic;
using System.Net;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.OAuth.ChannelElements;
using FloydPink.Flickr.Downloadr.Listener;
using FloydPink.Flickr.Downloadr.Model;

namespace FloydPink.Flickr.Downloadr.OAuth
{
    public class OAuthManager : IOAuthManager
    {
        private IHttpListenerManager _listenerManager;

        private string responseString = "<html><body>You have been authenticated. Please return to Flickr Downloadr.<br />" +
            "You could close this window at anytime.</body></html>";

        private DesktopConsumer Consumer { get; set; }
        private string RequestToken = string.Empty;
        private string AccessToken = string.Empty;

        private string ConsumerKey { get { return "33fe2dc1389339c4e9cd77e9a90ebabf"; } }
        private string ConsumerSecret { get { return "573233c34efdd943"; } }

        private MessageReceivingEndpoint FlickrEndPoint = new MessageReceivingEndpoint("http://api.flickr.com/services/rest", HttpDeliveryMethods.PostRequest);
        private ServiceProviderDescription FlickrServiceDescription
        {
            get
            {
                return new ServiceProviderDescription
                {
                    ProtocolVersion = DotNetOpenAuth.OAuth.ProtocolVersion.V10a,
                    RequestTokenEndpoint = new MessageReceivingEndpoint("http://www.flickr.com/services/oauth/request_token", HttpDeliveryMethods.PostRequest),
                    UserAuthorizationEndpoint = new MessageReceivingEndpoint("http://www.flickr.com/services/oauth/authorize", HttpDeliveryMethods.GetRequest),
                    AccessTokenEndpoint = new MessageReceivingEndpoint("http://www.flickr.com/services/oauth/access_token", HttpDeliveryMethods.GetRequest),
                    TamperProtectionElements = new ITamperProtectionChannelBindingElement[] { new HmacSha1SigningBindingElement() }
                };

            }
        }

        public OAuthManager(IHttpListenerManager listenerManager)
        {
            _listenerManager = listenerManager;
            Consumer = new DesktopConsumer(FlickrServiceDescription, new TokenManager(ConsumerKey, ConsumerSecret));
        }

        public string BeginAuthorization()
        {
            _listenerManager.RequestReceived += new EventHandler<HttpListenerCallbackEventArgs>(callbackManager_OnRequestReceived);
            _listenerManager.ResponseString = responseString;
            _listenerManager.SetupCallback();
            var requestArgs = new Dictionary<string, string>() { 
                { "oauth_callback", _listenerManager.ListenerAddress }
            };
            var redirectArgs = new Dictionary<string, string>() { 
                { "perms", "write" } 
            };
            return this.Consumer.RequestUserAuthorization(requestArgs, redirectArgs, out this.RequestToken).AbsoluteUri;
        }

        public event EventHandler<AuthenticatedEventArgs> Authenticated;

        public HttpWebRequest PrepareAuthorizedRequest(IDictionary<string, string> parameters)
        {
            return Consumer.PrepareAuthorizedRequest(FlickrEndPoint, this.AccessToken, parameters);
        }

        private string CompleteAuthorization(string verifier)
        {
            var response = this.Consumer.ProcessUserAuthorization(this.RequestToken, verifier);
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
