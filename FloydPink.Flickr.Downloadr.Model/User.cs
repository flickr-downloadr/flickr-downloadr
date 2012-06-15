using System.ComponentModel;
using System.Web.Script.Serialization;
using FloydPink.Flickr.Downloadr.Model.Extensions;

namespace FloydPink.Flickr.Downloadr.Model
{
    public class User : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _name;
        private string _userName;
        private string _userNsId;
        [ScriptIgnore] private UserInfo _info;

        public User()
        {
            _name = string.Empty;
            _userName = string.Empty;
            _userNsId = string.Empty;
        }

        public User(string name, string userName, string userNsId)
        {
            _name = name;
            _userName = userName;
            _userNsId = userNsId;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                PropertyChanged.Notify(() => this.Name);
            }
        }

        public string Username
        {
            get { return _userName; }
            set
            {
                _userName = value;
                PropertyChanged.Notify(() => this.Username);
            }
        }

        public string UserNsId
        {
            get { return _userNsId; }
            set
            {
                _userNsId = value;
                PropertyChanged.Notify(() => this.UserNsId);
            }
        }

        [ScriptIgnore]
        public UserInfo Info
        {
            get { return _info; }
            set
            {
                _info = value;
                PropertyChanged.Notify(() => this.Info);
            }
        }
    }
}