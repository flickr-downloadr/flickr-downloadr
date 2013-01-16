using StructureMap;

namespace FloydPink.Flickr.Downloadr.Bootstrap
{
    public static class Bootstrapper
    {
        private static Container _container;

        public static void Initialize()
        {
            _container = new Container(expression =>
                                           {
                                               expression.AddRegistry<CommonsRegistry>();
                                               expression.AddRegistry<OAuthRegistry>();
                                               expression.AddRegistry<RepositoryRegistry>();
                                               expression.AddRegistry<LogicRegistry>();
                                               expression.AddRegistry<PresentationRegistry>();
                                           });
        }

        public static T GetInstance<T>()
        {
            return _container.GetInstance<T>();
        }

        public static TPresenter GetPresenter<TView, TPresenter>(TView view)
        {
            return _container.With(view).GetInstance<TPresenter>();
        }
    }
}