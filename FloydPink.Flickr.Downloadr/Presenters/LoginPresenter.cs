using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FloydPink.Flickr.Downloadr.Views;
using FloydPink.Flickr.Downloadr.Repository;
using FloydPink.Flickr.Downloadr.OAuth;
using System.Diagnostics;

namespace FloydPink.Flickr.Downloadr.Presenters
{
    public class LoginPresenter : PresenterBase
    {
        private readonly ILoginView _view;
        private readonly IOAuthManager _oAuthManager;
        private readonly ITokenRepository _repository;

        public LoginPresenter(ILoginView view, IOAuthManager oAuthManager, ITokenRepository repository)
        {
            _view = view;
            _oAuthManager = oAuthManager;
            _repository = repository;
        }

        public void InitializeScreen()
        {
            if (string.IsNullOrEmpty(_repository.Get().TokenString))
            {
                _view.ShowLoggedOutControl();
            }
            else
            {
                _view.ShowLoggedInControl();
            }
        }

        public void LoginButtonClick()
        {
            _oAuthManager.Authenticated += new EventHandler<AuthenticatedEventArgs>(OAuthManager_Authenticated);
            Process.Start(new ProcessStartInfo()
            {
                FileName = _oAuthManager.BeginAuthorization()
            });
        }

        void OAuthManager_Authenticated(object sender, AuthenticatedEventArgs e)
        {
            _view.User = e.AuthenticatedUser;
            //_view.ShowLoggedInControl();
        }

        public void LogoutButtonClick()
        {
            _repository.Delete();
            _view.ShowLoggedOutControl();
        }

    }
}
