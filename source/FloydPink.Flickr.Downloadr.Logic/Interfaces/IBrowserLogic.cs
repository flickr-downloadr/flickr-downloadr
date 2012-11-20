using System.Collections.Generic;
using FloydPink.Flickr.Downloadr.Model;

namespace FloydPink.Flickr.Downloadr.Logic.Interfaces
{
    public interface IBrowserLogic
    {
        PhotosResponse GetPublicPhotos(User user, int page = 1);
    }
}