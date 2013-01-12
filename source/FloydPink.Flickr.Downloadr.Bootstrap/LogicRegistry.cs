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
            For<ILoginLogic>().Use<LoginLogic>().
                EnrichWith(s => DynamicProxyHelper.CreateInterfaceProxyWithTargetInterface(typeof (ILoginLogic), s));
            For<IBrowserLogic>().Use<BrowserLogic>().
                EnrichWith(s => DynamicProxyHelper.CreateInterfaceProxyWithTargetInterface(typeof (IBrowserLogic), s)).
                Ctor<int>("defaultPerPageCount").Is(DefaultPerPageCount);
            For<IDownloadLogic>().Use<DownloadLogic>().
                EnrichWith(s => DynamicProxyHelper.CreateInterfaceProxyWithTargetInterface(typeof (IDownloadLogic), s)).
                Ctor<string>("downloadLocation").Is(DefaultDownloadLocation);
        }
    }
}