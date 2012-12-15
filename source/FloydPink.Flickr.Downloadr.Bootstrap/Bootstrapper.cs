using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.OAuth.ChannelElements;
using FloydPink.Flickr.Downloadr.Logic;
using FloydPink.Flickr.Downloadr.Logic.Interfaces;
using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.OAuth;
using FloydPink.Flickr.Downloadr.OAuth.Listener;
using FloydPink.Flickr.Downloadr.Presentation;
using FloydPink.Flickr.Downloadr.Repository;
using StructureMap;


namespace FloydPink.Flickr.Downloadr.Bootstrap
{
    public static class Bootstrapper
    {
        private static string ConsumerKey = "33fe2dc1389339c4e9cd77e9a90ebabf";
        private static string ConsumerSecret = "573233c34efdd943";

        private static int DefaultPerPageCount = 50;
        private static string DefaultDownloadLocation = ".";
        
        private static readonly ServiceProviderDescription FlickrServiceDescription = new ServiceProviderDescription
                {
                    ProtocolVersion = ProtocolVersion.V10a,
                    RequestTokenEndpoint = new MessageReceivingEndpoint("http://www.flickr.com/services/oauth/request_token", HttpDeliveryMethods.PostRequest),
                    UserAuthorizationEndpoint = new MessageReceivingEndpoint("http://www.flickr.com/services/oauth/authorize", HttpDeliveryMethods.GetRequest),
                    AccessTokenEndpoint = new MessageReceivingEndpoint("http://www.flickr.com/services/oauth/access_token", HttpDeliveryMethods.GetRequest),
                    TamperProtectionElements = new ITamperProtectionChannelBindingElement[] { new HmacSha1SigningBindingElement() }
                };

        private static readonly MessageReceivingEndpoint FlickrServiceEndPoint =
            new MessageReceivingEndpoint("http://api.flickr.com/services/rest", HttpDeliveryMethods.PostRequest);

        public static void Load()
        {
            ObjectFactory.Initialize(initializer =>
                                         {
                                             initializer.For<ILoginLogic>().Use<LoginLogic>();
                                             initializer.For<IBrowserLogic>().Use<BrowserLogic>().
                                                 Ctor<int>("defaultPerPageCount").Is(DefaultPerPageCount);
                                             initializer.For<IDownloadLogic>().Use<DownloadLogic>().
                                                         Ctor<string>("downloadLocation").Is(DefaultDownloadLocation);

                                             initializer.For<IOAuthManager>().Singleton().Use<OAuthManager>().
                                                 Ctor<MessageReceivingEndpoint>("serviceEndPoint").Is(
                                                     FlickrServiceEndPoint);
                                             initializer.For<DesktopConsumer>().Use<DesktopConsumer>().
                                                 Ctor<ServiceProviderDescription>("serviceDescription").Is(
                                                     FlickrServiceDescription);
                                             initializer.For<IConsumerTokenManager>().Use<TokenManager>().
                                                 Ctor<string>("consumerKey").Is(ConsumerKey).
                                                 Ctor<string>("consumerSecret").Is(ConsumerSecret);

                                             initializer.For<IHttpListenerManager>().Use<HttpListenerManager>();

                                             initializer.For<IRepository<Token>>().Use<TokenRepository>();
                                             initializer.For<IRepository<User>>().Use<UserRepository>();
                                         });
        }

        public static T GetInstance<T>()
        {
            return ObjectFactory.GetInstance<T>();
        }

        public static TPresenter GetPresenter<TView, TPresenter>(TView view) where TPresenter : PresenterBase
        {
            return ObjectFactory.With(view).GetInstance<TPresenter>();
        }
    }
}