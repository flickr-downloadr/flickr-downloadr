using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FloydPink.Flickr.Downloadr.Logic.Interfaces;
using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Repository.Extensions;

namespace FloydPink.Flickr.Downloadr.Logic
{
    public class DownloadLogic : IDownloadLogic
    {
        private static readonly Random Random = new Random((int) DateTime.Now.Ticks);
        private readonly string _downloadLocation;
        private string _currentTimestampFolder;

        public DownloadLogic(string downloadLocation)
        {
            _downloadLocation = downloadLocation;
        }

        public async Task Download(IEnumerable<Photo> photos, CancellationToken cancellationToken,
                                   IProgress<ProgressUpdate> progress)
        {
            await DownloadAndSavePhotos(photos, cancellationToken, progress);
        }

        private async Task DownloadAndSavePhotos(IEnumerable<Photo> photos, CancellationToken cancellationToken,
                                                 IProgress<ProgressUpdate> progress)
        {
            try
            {
                //TODO: refactor this method !
                var progressUpdate = new ProgressUpdate
                                         {
                                             Cancellable = true,
                                             OperationText = "Downloading photos...",
                                             PercentDone = 0,
                                             ShowPercent = true
                                         };
                progress.Report(progressUpdate);

                int doneCount = 0;
                IList<Photo> photosList = photos as IList<Photo> ?? photos.ToList();
                int totalCount = photosList.Count();

                _currentTimestampFolder = GetSafeFilename(DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
                DirectoryInfo imageDirectory =
                    Directory.CreateDirectory(Path.Combine(_downloadLocation, _currentTimestampFolder));

                foreach (Photo photo in photosList)
                {
                    string targetFileName = Path.Combine(imageDirectory.FullName,
                                                         string.Format("{0}.{1}", GetSafeFilename(photo.Title),
                                                                       photo.DownloadFormat));
                    var metadata = new {photo.Title, photo.Description, photo.Tags};
                    File.WriteAllText(string.Format("{0}.json", targetFileName), metadata.ToJson(), Encoding.Unicode);

                    WebRequest request = WebRequest.Create(photo.LargestAvailableSizeUrl);

                    var buffer = new byte[4096];

                    using (var target = new FileStream(targetFileName, FileMode.Create, FileAccess.Write))
                    {
                        using (WebResponse response = await request.GetResponseAsync())
                        {
                            using (Stream stream = response.GetResponseStream())
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
                    progressUpdate.PercentDone = doneCount*100/totalCount;
                    progressUpdate.DownloadedPath = imageDirectory.FullName;
                    progress.Report(progressUpdate);
                    if (progressUpdate.PercentDone != 100) cancellationToken.ThrowIfCancellationRequested();
                }
            }
            catch (OperationCanceledException e)
            {
            }
        }

        private static string RandomString(int size)
        {
            // http://stackoverflow.com/a/1122519/218882
            var builder = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                char ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26*Random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        private static string GetSafeFilename(string path)
        {
            // http://stackoverflow.com/a/333297/218882
            string safeFilename = Path.GetInvalidFileNameChars()
                                      .Aggregate(path, (current, c) => current.Replace(c, '-'));
            return string.IsNullOrWhiteSpace(safeFilename) ? RandomString(8) : safeFilename;
        }
    }
}