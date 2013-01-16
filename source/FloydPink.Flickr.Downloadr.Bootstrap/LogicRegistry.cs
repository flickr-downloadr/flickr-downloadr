using FloydPink.Flickr.Downloadr.Logic;
using FloydPink.Flickr.Downloadr.Logic.Interfaces;
using StructureMap.Configuration.DSL;

namespace FloydPink.Flickr.Downloadr.Bootstrap
{
    public class LogicRegistry : Registry
    {
        private const int DefaultPerPageCount = 50;
        private const string DefaultDownloadLocation = ".";

        public LogicRegistry()
        {
            For<ILoginLogic>()
                .EnrichAllWith(DynamicProxy.LoggingInterceptorFor<ILoginLogic>())
                .Use<LoginLogic>();
            For<IBrowserLogic>()
                .EnrichAllWith(DynamicProxy.LoggingInterceptorFor<IBrowserLogic>())
                .Use<BrowserLogic>()
                .Ctor<int>("defaultPerPageCount").Is(DefaultPerPageCount);
            For<IDownloadLogic>()
                .EnrichAllWith(DynamicProxy.LoggingInterceptorFor<IDownloadLogic>())
                .Use<DownloadLogic>()
                .Ctor<string>("downloadLocation").Is(DefaultDownloadLocation);
        }
    }
}