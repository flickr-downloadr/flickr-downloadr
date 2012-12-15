using FloydPink.Flickr.Downloadr.Logic.Interfaces;
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

        public async void InitializeScreen()
        {
            _view.ShowSpinner(true);
            _view.ShowLoggedOutControl();
            if (!await _logic.IsUserLoggedInAsync(ApplyUser))
            {
                Logout();
            }
        }

        public void Login()
        {
            _view.ShowSpinner(true);
            _logic.Login(ApplyUser);
        }

        public void Logout()
        {
            _logic.Logout();
            _view.ShowSpinner(false);
            _view.ShowLoggedOutControl();
        }

        private void ApplyUser(User user)
        {
            _view.User = user;
            _view.ShowSpinner(false);
            _view.ShowLoggedInControl();
        }
    }
}