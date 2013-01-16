using FloydPink.Flickr.Downloadr.Presentation;
using StructureMap.Configuration.DSL;

namespace FloydPink.Flickr.Downloadr.Bootstrap
{
    public class PresentationRegistry : Registry
    {
        public PresentationRegistry()
        {
            For<ILoginPresenter>().EnrichAllWith(presenter => DynamicProxyHelper.CreateInterfaceProxyWithTargetInterface(
                typeof (ILoginPresenter), presenter)).Use<LoginPresenter>();
            For<IBrowserPresenter>().EnrichAllWith(presenter => DynamicProxyHelper.CreateInterfaceProxyWithTargetInterface(
                typeof (IBrowserPresenter), presenter)).Use<BrowserPresenter>();
        }
    }
}