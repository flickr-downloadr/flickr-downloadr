using FloydPink.Flickr.Downloadr.Logic.Interfaces;
using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Presentation.Views;

namespace FloydPink.Flickr.Downloadr.Presentation
{
    public class PreferencesPresenter : PresenterBase, IPreferencesPresenter
    {
        private readonly IPreferencesLogic _logic;
        private IPreferencesView _view;

        public PreferencesPresenter(IPreferencesView view, IPreferencesLogic logic)
        {
            _view = view;
            _logic = logic;
        }

        public void Save(Preferences preferences)
        {
            _logic.SavePreferences(preferences);
        }
    }
}