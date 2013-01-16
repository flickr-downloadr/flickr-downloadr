using FloydPink.Flickr.Downloadr.Presentation;
using StructureMap.Configuration.DSL;

namespace FloydPink.Flickr.Downloadr.Bootstrap
{
    public class PresentationRegistry : Registry
    {
        public PresentationRegistry()
        {
            For<ILoginPresenter>()
                .EnrichAllWith(DynamicProxy.LoggingInterceptorFor<ILoginPresenter>())
                .Use<LoginPresenter>();
            For<IBrowserPresenter>()
                .EnrichAllWith(DynamicProxy.LoggingInterceptorFor<IBrowserPresenter>())
                .Use<BrowserPresenter>();
        }
    }
}