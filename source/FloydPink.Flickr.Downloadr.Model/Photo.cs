using System.Globalization;
using FloydPink.Flickr.Downloadr.Model.Constants;
using FloydPink.Flickr.Downloadr.Model.Enums;

namespace FloydPink.Flickr.Downloadr.Model
{
    public class Photo
    {
        private readonly int _farm;
        private readonly string _id;
        private readonly bool _isFamily;
        private readonly bool _isFriend;
        private readonly bool _isPublic;
        private readonly string _owner;
        private readonly string _secret;
        private readonly string _server;
        private readonly string _title;

        public Photo(string id, string owner, string secret, string server, int farm, string title, bool isPublic,
                     bool isFamily, bool isFriend)
        {
            _id = id;
            _owner = owner;
            _secret = secret;
            _server = server;
            _farm = farm;
            _title = title;
            _isPublic = isPublic;
            _isFamily = isFamily;
            _isFriend = isFriend;
        }

        public string Id
        {
            get { return _id; }
        }

        public string Owner
        {
            get { return _owner; }
        }

        public string Secret
        {
            get { return _secret; }
        }

        public string Server
        {
            get { return _server; }
        }

        public int Farm
        {
            get { return _farm; }
        }

        public string Title
        {
            get { return _title; }
        }

        public bool IsPublic
        {
            get { return _isPublic; }
        }

        public bool IsFamily
        {
            get { return _isFamily; }
        }

        public bool IsFriend
        {
            get { return _isFriend; }
        }

        public string LargeSquare150X150Url
        {
            get { return GetPhotoUrl(PhotoFormat.LargeSquare150X150); }
        }

        public string Small320Url
        {
            get { return GetPhotoUrl(PhotoFormat.Small320); }
        }

        public string OriginalUrl
        {
            get { return GetPhotoUrl(PhotoFormat.Original); }
        }

        private string GetPhotoUrl(string photoFormat)
        {
            return string.Format(AppConstants.PhotoUrlFormat, Farm.ToString(CultureInfo.InvariantCulture), Server, Id,
                                 Secret, photoFormat, "jpg");
        }
    }
}