using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using FloydPink.Flickr.Downloadr.Logic.Extensions;
using FloydPink.Flickr.Downloadr.Logic.Interfaces;
using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Model.Constants;
using FloydPink.Flickr.Downloadr.OAuth;
using FloydPink.Flickr.Downloadr.Repository;
using log4net;

namespace FloydPink.Flickr.Downloadr.Logic
{
    public class LoginLogic : ILoginLogic
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (LoginLogic));
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

        #region ILoginLogic Members

        public void Login(Action<User> applyUser)
        {
            Log.Debug("Entering Login Method.");

            _applyUser = applyUser;
            _oAuthManager.Authenticated += OAuthManagerAuthenticated;
            Process.Start(new ProcessStartInfo
                              {
                                  FileName = _oAuthManager.BeginAuthorization()
                              });

            Log.Debug("Leaving Login Method.");
        }

        public void Logout()
        {
            Log.Debug("Entering Logout Method.");

            _tokenRepository.Delete();
            _userRepository.Delete();

            Log.Debug("Leaving Logout Method.");
        }

        public async Task<bool> IsUserLoggedInAsync(Action<User> applyUser)
        {
            Log.Debug("Entering IsUserLoggedInAsync Method.");

            _applyUser = applyUser;
            Token token = _tokenRepository.Get();
            User user = _userRepository.Get();
            if (string.IsNullOrEmpty(token.TokenString))
            {
                return false;
            }

            _oAuthManager.AccessToken = token.TokenString;
            var testLogin =
                (Dictionary<string, object>) await _oAuthManager.MakeAuthenticatedRequestAsync(Methods.TestLogin);
            bool userIsLoggedIn = (string) testLogin.GetValueFromDictionary("user", "id") == user.UserNsId;

            if (userIsLoggedIn)
            {
                CallApplyUser(user);
            }
            Log.Debug("Leaving IsUserLoggedInAsync Method.");

            return userIsLoggedIn;
        }

        #endregion

        private void OAuthManagerAuthenticated(object sender, AuthenticatedEventArgs e)
        {
            Log.Debug("Entering OAuthManagerAuthenticated Method.");

            User authenticatedUser = e.AuthenticatedUser;
            _userRepository.Save(authenticatedUser);
            CallApplyUser(authenticatedUser);

            Log.Debug("Leaving OAuthManagerAuthenticated Method.");
        }

        private async void CallApplyUser(User authenticatedUser)
        {
            Log.Debug("Entering CallApplyUser Method.");

            var exraParams = new Dictionary<string, string>
                                 {
                                     {ParameterNames.UserId, authenticatedUser.UserNsId}
                                 };
            var userInfo = (Dictionary<string, object>)
                           (await _oAuthManager.MakeAuthenticatedRequestAsync(Methods.PeopleGetInfo, exraParams))[
                               "person"];
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

            Log.Debug("Leaving CallApplyUser Method.");
        }
    }
}