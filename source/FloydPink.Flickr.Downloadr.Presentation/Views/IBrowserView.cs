using FloydPink.Flickr.Downloadr.Model;
using System.Collections.ObjectModel;

namespace FloydPink.Flickr.Downloadr.Presentation.Views
{
    public interface IBrowserView : IBaseView
    {
        User User { get; set; }

        ObservableCollection<Photo> Photos { get; set; }
    }
}