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
        private bool _titleAsFilename;
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

        public bool TitleAsFilename
        {
            get { return _titleAsFilename; }
            set
            {
                _titleAsFilename = value;
                PropertyChanged.Notify(() => TitleAsFilename);
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

        public PhotoDownloadSize DownloadSize
        {
            get { return _downloadSize; }
            set
            {
                _downloadSize = value;
                PropertyChanged.Notify(() => DownloadSize);
            }
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

        public ObservableCollection<string> Metadata { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public static Preferences GetDefault()
        {
            return new Preferences
                       {
                           TitleAsFilename = false,
                           PhotosPerPage = 25,
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