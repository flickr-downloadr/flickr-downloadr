using System.Collections.Generic;
using System.Threading.Tasks;
using FloydPink.Flickr.Downloadr.Logic.Interfaces;
using FloydPink.Flickr.Downloadr.Model;
using log4net;

namespace FloydPink.Flickr.Downloadr.Logic
{
    public class DownloadLogic : IDownloadLogic
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (DownloadLogic));
        private readonly string _downloadLocation;

        public DownloadLogic(string downloadLocation)
        {
            _downloadLocation = downloadLocation;
        }

        public async Task Download(IEnumerable<Photo> photos)
        {
            Log.Debug("In Download Method.");

            await Task.Delay(1000);
        }
    }
}