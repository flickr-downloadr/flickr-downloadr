using System.Collections.Generic;
using System.Collections.ObjectModel;
using FloydPink.Flickr.Downloadr.Model;

namespace FloydPink.Flickr.Downloadr.Presentation.Views
{
    public interface IBrowserView : IBaseView
    {
        User User { get; set; }
        ObservableCollection<Photo> Photos { get; set; }
        IList<Photo> SelectedPhotos { get; set; }

        string Page { get; set; }
        string Pages { get; set; }
        string PerPage { get; set; }
        string Total { get; set; }
    }
}