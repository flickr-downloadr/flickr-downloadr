using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FloydPink.Flickr.Downloadr.Logic.Interfaces;
using FloydPink.Flickr.Downloadr.Model;


namespace FloydPink.Flickr.Downloadr.Logic
{
    public class DownloadLogic : IDownloadLogic
    {
        private readonly string _downloadLocation;
        private string _currentTimestampFolder;

        public DownloadLogic(string downloadLocation)
        {
            _downloadLocation = downloadLocation;
        }

        public async Task Download(IEnumerable<Photo> photos, CancellationToken cancellationToken, IProgress<int> progress)
        {
            progress.Report(0);

            var doneCount = 0;
            var photosList = photos as IList<Photo> ?? photos.ToList();
            var totalCount = photosList.Count();

            _currentTimestampFolder = GetSafeFilename(DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
            var imageDirectory = Directory.CreateDirectory(Path.Combine(_downloadLocation, _currentTimestampFolder));

            foreach (var photo in photosList)
            {
                var targetFileName = Path.Combine(imageDirectory.FullName, string.Format("{0}.{1}",
                    GetSafeFilename(photo.Title), photo.OriginalFormat));

                var request = WebRequest.Create(photo.OriginalUrl);

                var buffer = new byte[4096];

                using (var target = new FileStream(targetFileName, FileMode.Create, FileAccess.Write))
                {
                    using (var response = await request.GetResponseAsync())
                    {
                        using (var stream = response.GetResponseStream())
                        {
                            int read;
                            while ((read = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                            {
                                await target.WriteAsync(buffer, 0, read);
                            }
                        }
                    }
                }

                doneCount++;
                progress.Report(doneCount * 100 / totalCount);
            }

        }

        private string GetSafeFilename(string path)
        {
            // http://stackoverflow.com/a/333297/218882
            return Path.GetInvalidFileNameChars().Aggregate(path, (current, c) => current.Replace(c, '-'));
        }
    }
}