using FloydPink.Flickr.Downloadr.Logic;
using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Presentation.Views;

namespace FloydPink.Flickr.Downloadr.Presentation
{
    public class LoginPresenter : PresenterBase
    {
        private readonly ILoginLogic _logic;
        private readonly ILoginView _view;

        public LoginPresenter(ILoginView view, ILoginLogic logic)
        {
            _view = view;
            _logic = logic;
        }

        public void InitializeScreen()
        {
            _view.ShowLoggedOutControl();
            if (!_logic.IsUserLoggedIn(ApplyUser))
            {
                Logout();
            }
        }

        public void Login()
        {
            _logic.Login(ApplyUser);
        }

        public void Logout()
        {
            _logic.Logout();
            _view.ShowLoggedOutControl();
        }

        private void ApplyUser(User user)
        {
            _view.User = user;
            _view.ShowLoggedInControl();
        }
    }
}