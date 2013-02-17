using FloydPink.Flickr.Downloadr.Logic.Interfaces;
using FloydPink.Flickr.Downloadr.Model;

namespace FloydPink.Flickr.Downloadr.Presentation
{
    public class PreferencesPresenter : PresenterBase, IPreferencesPresenter
    {
        private readonly IPreferencesLogic _logic;

        public PreferencesPresenter(IPreferencesLogic logic)
        {
            _logic = logic;
        }

        public void Save(Preferences preferences)
        {
            _logic.SavePreferences(preferences);
        }
    }
}