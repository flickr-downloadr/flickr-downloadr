using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetOpenAuth.OAuth;
using FloydPink.Flickr.Downloadr.Services;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth.ChannelElements;
using System.Net;

namespace FloydPink.Flickr.Downloadr.OAuth
{
    public class OAuthManager
    {
        private DesktopConsumer consumer { get; set; }
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

        public OAuthManager()
        {
            consumer = new DesktopConsumer(FlickrServiceDescription, new TokenManager(ConsumerKey, ConsumerSecret));
        }

        public string RequestUserAuthorization()
        {
            var requestArgs = new Dictionary<string, string>();
            return this.consumer.RequestUserAuthorization(requestArgs, null, out this.RequestToken).AbsoluteUri;
        }

        public string ProcessUserAuthorization(string verifier)
        {
            var response = this.consumer.ProcessUserAuthorization(this.RequestToken, verifier);
            this.AccessToken = response.AccessToken;
            return response.AccessToken;
        }

        public HttpWebRequest PrepareAuthorizedRequest(IDictionary<string, string> parameters)
        {
            return consumer.PrepareAuthorizedRequest(FlickrEndPoint, this.AccessToken, parameters);
        }


    }
}
