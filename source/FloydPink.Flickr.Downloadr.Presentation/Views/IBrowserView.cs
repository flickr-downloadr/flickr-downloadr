using System.Collections.ObjectModel;
using FloydPink.Flickr.Downloadr.Model;

namespace FloydPink.Flickr.Downloadr.Presentation.Views
{
    public interface IBrowserView : IBaseView
    {
        User User { get; set; }
        ObservableCollection<Photo> Photos { get; set; }
    }
}