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
            For<ILoginLogic>().EnrichAllWith(s => DynamicProxyHelper.CreateInterfaceProxyWithTargetInterface(typeof (ILoginLogic), s)).Use<LoginLogic>();
            For<IBrowserLogic>().EnrichAllWith(s => DynamicProxyHelper.CreateInterfaceProxyWithTargetInterface(typeof(IBrowserLogic), s)).Use<BrowserLogic>().
                                 Ctor<int>("defaultPerPageCount").Is(DefaultPerPageCount);
            For<IDownloadLogic>().EnrichAllWith(s => DynamicProxyHelper.CreateInterfaceProxyWithTargetInterface(typeof(IDownloadLogic), s)).Use<DownloadLogic>().
                                  Ctor<string>("downloadLocation").Is(DefaultDownloadLocation);
        }
    }
}