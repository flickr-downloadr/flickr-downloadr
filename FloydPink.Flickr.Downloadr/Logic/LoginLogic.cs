using System;
using System.Collections.Generic;
using System.Diagnostics;
using FloydPink.Flickr.Downloadr.Extensions;
using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Model.Constants;
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

        public LoginLogic(IOAuthManager oAuthManager, IRepository<Token> tokenRepository,
                          IRepository<User> userRepository)
        {
            _oAuthManager = oAuthManager;
            _tokenRepository = tokenRepository;
            _userRepository = userRepository;
        }

        public void Login(Action<User> applyUser)
        {
            _applyUser = applyUser;
            _oAuthManager.Authenticated += OAuthManagerAuthenticated;
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
            _applyUser = applyUser;
            var token = _tokenRepository.Get();
            var user = _userRepository.Get();
            if (string.IsNullOrEmpty(token.TokenString))
            {
                return false;
            }

            _oAuthManager.AccessToken = token.TokenString;
            var testLogin = (Dictionary<string, object>) _oAuthManager.MakeAuthenticatedRequest(Methods.TestLogin);
            var userIsLoggedIn = (string) testLogin.GetValueFromDictionary("user", "id") == user.UserNsId;

            if (userIsLoggedIn)
            {
                CallApplyUser(user);
            }
            return userIsLoggedIn;
        }

        private void OAuthManagerAuthenticated(object sender, AuthenticatedEventArgs e)
        {
            User authenticatedUser = e.AuthenticatedUser;
            _userRepository.Save(authenticatedUser);
            CallApplyUser(authenticatedUser);
        }

        private void CallApplyUser(User authenticatedUser)
        {
            var exraParams = new Dictionary<string, string>
                                 {
                                     {ParameterNames.UserId, authenticatedUser.UserNsId}
                                 };
            var userInfo =
                (Dictionary<string, object>)
                _oAuthManager.MakeAuthenticatedRequest(Methods.PeopleGetInfo, exraParams)["person"];
            authenticatedUser.Info = new UserInfo
                                         {
                                             Id = authenticatedUser.UserNsId,
                                             IsPro = Convert.ToBoolean(userInfo["ispro"]),
                                             IconServer = userInfo["iconserver"].ToString(),
                                             IconFarm = Convert.ToInt32(userInfo["iconfarm"]),
                                             PathAlias =
                                                 userInfo["path_alias"] == null
                                                     ? string.Empty
                                                     : userInfo["path_alias"].ToString(),
                                             Description = userInfo.GetValueFromDictionary("description").ToString(),
                                             PhotosUrl = userInfo.GetValueFromDictionary("photosurl").ToString(),
                                             ProfileUrl = userInfo.GetValueFromDictionary("profileurl").ToString(),
                                             MobileUrl = userInfo.GetValueFromDictionary("mobileurl").ToString(),
                                             PhotosCount =
                                                 Convert.ToInt32(
                                                     ((Dictionary<string, object>) userInfo["photos"]).
                                                         GetValueFromDictionary("count"))
                                         };
            _applyUser(authenticatedUser);
        }
    }
}