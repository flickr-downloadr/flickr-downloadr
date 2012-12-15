using System.Collections.Generic;
using System.Threading.Tasks;
using FloydPink.Flickr.Downloadr.Logic.Interfaces;
using FloydPink.Flickr.Downloadr.Model;


namespace FloydPink.Flickr.Downloadr.Logic
{
    public class DownloadLogic : IDownloadLogic
    {
        private readonly string _downloadLocation;

        public DownloadLogic(string downloadLocation)
        {
            _downloadLocation = downloadLocation;
        }

        public async Task Download(IEnumerable<Photo> photos)
        {
            await Task.Delay(1000);
        }
    }
}