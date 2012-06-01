using System;
using System.Collections.Generic;
using System.Diagnostics;
using FloydPink.Flickr.Downloadr.Extensions;
using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.OAuth;
using FloydPink.Flickr.Downloadr.Repository;

namespace FloydPink.Flickr.Downloadr.Logic
{
    public class LoginLogic : ILoginLogic
    {
        private readonly IOAuthManager _oAuthManager;
        private readonly IRepository<Token> _tokenRepository;
        private readonly IRepository<User> _userRepository;
        private Action<User> _applyUser;

        public LoginLogic(IOAuthManager oAuthManager, IRepository<Token> tokenRepository, IRepository<User> userRepository)
        {
            _oAuthManager = oAuthManager;
            _tokenRepository = tokenRepository;
            _userRepository = userRepository;
        }

        public void Login(Action<User> applyUser)
        {
            _applyUser = applyUser;
            _oAuthManager.Authenticated += new EventHandler<AuthenticatedEventArgs>(OAuthManager_Authenticated);
            Process.Start(new ProcessStartInfo()
            {
                FileName = _oAuthManager.BeginAuthorization()
            });
        }

        public void Logout()
        {
            _tokenRepository.Delete();
            _userRepository.Delete();
        }

        public bool IsUserLoggedIn(Action<User> applyUser)
        {
            var token = _tokenRepository.Get();
            var user = _userRepository.Get();
            if (string.IsNullOrEmpty(token.TokenString))
            {
                return false;
            }

            _oAuthManager.AccessToken = token.TokenString;
            var testLogin = (Dictionary<string, object>)_oAuthManager.MakeAuthenticatedRequest("flickr.test.login");
            var userIsLoggedIn = (string)testLogin.GetValueFromDictionary("user", "id") == user.UserNSId;

            if (userIsLoggedIn)
            {
                applyUser(user);
            }
            return userIsLoggedIn;
        }

        void OAuthManager_Authenticated(object sender, AuthenticatedEventArgs e)
        {
            _userRepository.Save(e.AuthenticatedUser);
            _applyUser(e.AuthenticatedUser);
        }
    }
}
