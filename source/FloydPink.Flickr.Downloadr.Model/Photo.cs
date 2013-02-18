namespace FloydPink.Flickr.Downloadr.Model
{
    public class Photo
    {
        private readonly string _description;
        private readonly int _farm;
        private readonly string _id;
        private readonly bool _isFamily;
        private readonly bool _isFriend;
        private readonly bool _isPublic;
        private readonly string _large1024Url;
        private readonly string _largeSquare150X150Url;
        private readonly string _medium500Url;
        private readonly string _medium640Url;
        private readonly string _medium800Url;
        private readonly string _originalFormat;
        private readonly string _originalSecret;
        private readonly string _originalUrl;
        private readonly string _owner;
        private readonly string _secret;
        private readonly string _server;
        private readonly string _small240Url;
        private readonly string _small320Url;
        private readonly string _smallSquare75X75Url;
        private readonly string _tags;
        private readonly string _thumbnailUrl;
        private readonly string _title;

        public Photo(string id, string owner, string secret, string server, int farm, string title, bool isPublic,
                     bool isFamily, bool isFriend,
                     string description, string tags, string originalSecret, string originalFormat,
                     string smallSquare75X75Url, string largeSquare150X150Url,
                     string thumbnailUrl, string small240Url, string small320Url, string medium500Url,
                     string medium640Url, string medium800Url, string large1024Url,
                     string originalUrl)
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
            _description = description;
            _tags = tags;
            _originalSecret = originalSecret;
            _originalFormat = originalFormat;
            _smallSquare75X75Url = smallSquare75X75Url;
            _largeSquare150X150Url = largeSquare150X150Url;
            _thumbnailUrl = thumbnailUrl;
            _small240Url = small240Url;
            _small320Url = small320Url;
            _medium500Url = medium500Url;
            _medium640Url = medium640Url;
            _medium800Url = medium800Url;
            _large1024Url = large1024Url;
            _originalUrl = originalUrl;
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

        public string Description
        {
            get { return _description; }
        }

        public string Tags
        {
            get { return _tags; }
        }

        public string OriginalSecret
        {
            get { return _originalSecret; }
        }

        public string OriginalFormat
        {
            get { return _originalFormat; }
        }

        public string SmallSquare75X75Url
        {
            get { return _smallSquare75X75Url; }
        }

        public string LargeSquare150X150Url
        {
            get { return _largeSquare150X150Url; }
        }

        public string ThumbnailUrl
        {
            get { return _thumbnailUrl; }
        }

        public string Small240Url
        {
            get { return _small240Url; }
        }

        public string Small320Url
        {
            get { return string.IsNullOrWhiteSpace(_small320Url) ? Small240Url : _small320Url; }
        }

        public string Medium500Url
        {
            get { return string.IsNullOrWhiteSpace(_medium500Url) ? Small320Url : _medium500Url; }
        }

        public string Medium640Url
        {
            get { return string.IsNullOrWhiteSpace(_medium640Url) ? Medium500Url : _medium640Url; }
        }

        public string Medium800Url
        {
            get { return string.IsNullOrWhiteSpace(_medium800Url) ? Medium640Url : _medium800Url; }
        }

        public string Large1024Url
        {
            get { return string.IsNullOrWhiteSpace(_large1024Url) ? Medium800Url : _large1024Url; }
        }

        public string OriginalUrl
        {
            get { return string.IsNullOrWhiteSpace(_originalUrl) ? Large1024Url : _originalUrl; }
        }

        public string DownloadFormat
        {
            get { return string.IsNullOrWhiteSpace(OriginalFormat) ? "jpg" : OriginalFormat; }
        }
    }
}