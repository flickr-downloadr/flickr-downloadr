using System.ComponentModel;
using FloydPink.Flickr.Downloadr.Extensions;

namespace FloydPink.Flickr.Downloadr.Model
{
    public class User : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _name;
        private string _userName;
        private string _userNSId;

        public User()
        {
            _name = string.Empty;
            _userName = string.Empty;
            _userNSId = string.Empty;
        }

        public User(string name, string userName, string userNSId)
        {
            _name = name;
            _userName = userName;
            _userNSId = userNSId;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                this._name = value;
                this.PropertyChanged.Notify(() => this.Name);
            }
        }
        public string Username
        {
            get { return _userName; }
            set
            {
                this._userName = value;
                this.PropertyChanged.Notify(() => this.Username);
            }
        }
        public string UserNSId
        {
            get { return _userNSId; }
            set
            {
                this._userNSId = value;
                this.PropertyChanged.Notify(() => this.UserNSId);
            }
        }
    }
}
