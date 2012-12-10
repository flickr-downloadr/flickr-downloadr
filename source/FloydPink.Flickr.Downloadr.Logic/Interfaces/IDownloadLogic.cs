using System.Collections.Generic;
using System.Threading.Tasks;
using FloydPink.Flickr.Downloadr.Model;

namespace FloydPink.Flickr.Downloadr.Logic.Interfaces
{
    public interface IDownloadLogic
    {
        Task Download(IEnumerable<Photo> photos);
    }
}