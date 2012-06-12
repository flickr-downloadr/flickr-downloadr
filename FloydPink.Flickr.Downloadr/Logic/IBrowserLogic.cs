using System.Collections.Generic;
using FloydPink.Flickr.Downloadr.Model;

namespace FloydPink.Flickr.Downloadr.Logic
{
    public interface IBrowserLogic
    {
        IEnumerable<Photo> GetPublicPhotos(User user, int page = 1);
    }
}