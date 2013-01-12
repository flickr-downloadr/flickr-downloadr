using FloydPink.Flickr.Downloadr.Presentation;
using StructureMap;

namespace FloydPink.Flickr.Downloadr.Bootstrap
{
    public static class Bootstrapper
    {
        public static void Load()
        {
            ObjectFactory.Initialize(initializer =>
                                         {
                                             initializer.AddRegistry<CommonsRegistry>();
                                             initializer.AddRegistry<OAuthRegistry>();
                                             initializer.AddRegistry<RepositoryRegistry>();
                                             initializer.AddRegistry<LogicRegistry>();
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