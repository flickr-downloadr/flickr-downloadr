using System;
using System.Collections.Generic;
using System.Diagnostics;
using FloydPink.Flickr.Downloadr.Extensions;
using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.OAuth;
using FloydPink.Flickr.Downloadr.Repository;
using FloydPink.Flickr.Downloadr.Views;

namespace FloydPink.Flickr.Downloadr.Presenters
{
    public class LoginPresenter : PresenterBase
    {
        private readonly ILoginView _view;
        private readonly IOAuthManager _oAuthManager;
        private readonly IRepository<Token> _tokenRepository;
        private readonly IRepository<User> _userRepository;

        public LoginPresenter(ILoginView view, IOAuthManager oAuthManager, IRepository<Token> tokenRepository, IRepository<User> userRepository)
        {
            _view = view;
            _oAuthManager = oAuthManager;
            _tokenRepository = tokenRepository;
            _userRepository = userRepository;
        }

        public void InitializeScreen()
        {
            var token = _tokenRepository.Get();
            var user = _userRepository.Get();
            _view.ShowLoggedOutControl();
            if (!string.IsNullOrEmpty(token.TokenString))
            {
                _oAuthManager.AccessToken = token.TokenString;
                var testLogin = (Dictionary<string, object>)_oAuthManager.MakeAuthenticatedRequest("flickr.test.login");
                if ((string)testLogin.GetValueFromDictionary("user", "id") == user.UserNSId)
                {
                    _view.User = user;
                    _view.ShowLoggedInControl();
                }
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
            _userRepository.Save(e.AuthenticatedUser);
            _view.ShowLoggedInControl();
        }

        public void LogoutButtonClick()
        {
            _tokenRepository.Delete();
            _userRepository.Delete();
            _view.ShowLoggedOutControl();
        }

    }
}
