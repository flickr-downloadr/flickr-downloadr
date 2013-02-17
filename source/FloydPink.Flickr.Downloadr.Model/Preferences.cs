using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using FloydPink.Flickr.Downloadr.Model.Enums;
using FloydPink.Flickr.Downloadr.Model.Extensions;

namespace FloydPink.Flickr.Downloadr.Model
{
    public class Preferences : INotifyPropertyChanged
    {
        private string _downloadLocation;
        private PhotoDownloadSize _downloadSize;
        private int _photosPerPage;

        public Preferences()
        {
        }

        public Preferences(int photosPerPage, string downloadLocation, List<string> metadata,
                           PhotoDownloadSize downloadSize)
        {
            _photosPerPage = photosPerPage;
            _downloadLocation = downloadLocation;
            Metadata = new ObservableCollection<string>(metadata);
            _downloadSize = downloadSize;
        }

        public int PhotosPerPage
        {
            get { return _photosPerPage; }
            set
            {
                _photosPerPage = value;
                PropertyChanged.Notify(() => PhotosPerPage);
            }
        }

        public string DownloadLocation
        {
            get { return _downloadLocation; }
            set
            {
                _downloadLocation = value;
                PropertyChanged.Notify(() => DownloadLocation);
            }
        }

        public ObservableCollection<string> Metadata { get; set; }

        public PhotoDownloadSize DownloadSize
        {
            get { return _downloadSize; }
            set
            {
                _downloadSize = value;
                PropertyChanged.Notify(() => DownloadSize);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static Preferences GetDefault()
        {
            return new Preferences
                       {
                           PhotosPerPage = 50,
                           DownloadLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                           Metadata =
                               new ObservableCollection<string>
                                   {
                                       PhotoMetadata.Title,
                                       PhotoMetadata.Description,
                                       PhotoMetadata.Tags
                                   },
                           DownloadSize = PhotoDownloadSize.Original
                       };
        }
    }
}