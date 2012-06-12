using FloydPink.Flickr.Downloadr.Model;

namespace FloydPink.Flickr.Downloadr.Views
{
    public interface ILoginView : IBaseView
    {
        User User { get; set; }

        void ShowLoggedInControl();
        void ShowLoggedOutControl();
    }
}