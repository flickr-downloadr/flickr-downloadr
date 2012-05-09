using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FloydPink.Flickr.Downloadr.Views;
using FloydPink.Flickr.Downloadr.Repository;
using FloydPink.Flickr.Downloadr.OAuth;

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
            _view.OpenAuthorizationUrl(_oAuthManager.RequestUserAuthorization());            
        }

        public void CompleteLoginProcess(string validatorToken)
        {
            _oAuthManager.ProcessUserAuthorization(validatorToken);
        }
    }
}
