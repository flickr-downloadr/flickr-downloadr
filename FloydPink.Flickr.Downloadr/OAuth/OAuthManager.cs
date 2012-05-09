using System;
using System.Collections.Generic;
using System.Net;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.OAuth.ChannelElements;
using FloydPink.Flickr.Downloadr.OAuth;
using FloydPink.Flickr.Downloadr.Model;

namespace FloydPink.Flickr.Downloadr.OAuth
{
    public class OAuthManager : IOAuthManager
    {
        private IOAuthCallbackManager _callbackManager;

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

        public OAuthManager(IOAuthCallbackManager callbackManager)
        {
            _callbackManager = callbackManager;
            Consumer = new DesktopConsumer(FlickrServiceDescription, new TokenManager(ConsumerKey, ConsumerSecret));
        }

        public string BeginAuthorization()
        {
            _callbackManager.OAuthCallbackEvent += new EventHandler<OAuthCallbackEventArgs>(callbackManager_OAuthCallbackEvent);
            _callbackManager.SetupCallback();
            var requestArgs = new Dictionary<string, string>() { { "oauth_callback", _callbackManager.ListenerAddress } };
            return this.Consumer.RequestUserAuthorization(requestArgs, null, out this.RequestToken).AbsoluteUri;
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

        private void callbackManager_OAuthCallbackEvent(object sender, OAuthCallbackEventArgs e)
        {
            if (e.Token == this.RequestToken)
            {
                CompleteAuthorization(e.Verifier);
            }
        }
    }
}
