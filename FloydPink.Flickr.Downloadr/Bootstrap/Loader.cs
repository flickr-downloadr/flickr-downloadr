using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;
using FloydPink.Flickr.Downloadr.OAuth;
using FloydPink.Flickr.Downloadr.Repository;
using FloydPink.Flickr.Downloadr.Presenters;

namespace FloydPink.Flickr.Downloadr.Bootstrap
{
    public static class Loader
    {
        public static void Load()
        {
            ObjectFactory.Initialize(initializer =>
                {
                    initializer.For<IOAuthManager>().Use<OAuthManager>();
                    initializer.For<ITokenRepository>().Use<AccessTokenRepository>();
                });
        }

        public static T GetInstance<T>()
        {
            return ObjectFactory.GetInstance<T>();
        }

        public static TPresenter GetPresenter<TView, TPresenter>(TView view) where TPresenter:PresenterBase
        {
            return ObjectFactory.With<TView>(view).GetInstance<TPresenter>();
        }
    }
}
