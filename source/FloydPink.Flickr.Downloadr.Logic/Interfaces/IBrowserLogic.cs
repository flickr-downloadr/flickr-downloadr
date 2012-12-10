using System.Threading.Tasks;
using FloydPink.Flickr.Downloadr.Model;

namespace FloydPink.Flickr.Downloadr.Logic.Interfaces
{
    public interface IBrowserLogic
    {
        Task<PhotosResponse> GetPublicPhotosAsync(User user, int page = 1);
    }
}