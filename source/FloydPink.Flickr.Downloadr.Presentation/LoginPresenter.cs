using FloydPink.Flickr.Downloadr.Logic.Interfaces;
using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Presentation.Views;
using log4net;

namespace FloydPink.Flickr.Downloadr.Presentation
{
    public class LoginPresenter : PresenterBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (LoginPresenter));
        private readonly ILoginLogic _logic;
        private readonly ILoginView _view;

        public LoginPresenter(ILoginView view, ILoginLogic logic)
        {
            _view = view;
            _logic = logic;
        }

        public async void InitializeScreen()
        {
            Log.Debug("Entering InitializeScreen Method.");

            _view.ShowSpinner(true);
            _view.ShowLoggedOutControl();
            if (!await _logic.IsUserLoggedInAsync(ApplyUser))
            {
                Logout();
            }

            Log.Debug("Leaving InitializeScreen Method.");
        }

        public void Login()
        {
            Log.Debug("Entering Login Method.");

            _view.ShowSpinner(true);
            _logic.Login(ApplyUser);

            Log.Debug("Leaving Login Method.");
        }

        public void Logout()
        {
            Log.Debug("Entering Logout Method.");

            _logic.Logout();
            _view.ShowSpinner(false);
            _view.ShowLoggedOutControl();

            Log.Debug("Leaving Logout Method.");
        }

        private void ApplyUser(User user)
        {
            Log.Debug("Entering ApplyUser Method.");

            _view.User = user;
            _view.ShowSpinner(false);
            _view.ShowLoggedInControl();

            Log.Debug("Leaving ApplyUser Method.");
        }
    }
}