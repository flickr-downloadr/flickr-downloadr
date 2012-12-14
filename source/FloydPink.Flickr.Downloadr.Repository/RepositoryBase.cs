using System.IO;
using FloydPink.Flickr.Downloadr.Repository.Helpers;
using log4net;

namespace FloydPink.Flickr.Downloadr.Repository
{
    public abstract class RepositoryBase
    {
        
        private static readonly ILog Log = LogManager.GetLogger(typeof(RepositoryBase));
        protected readonly string CryptKey = "SomeEncryPtionKey123";
        internal abstract string RepoFileName { get; }

        public void Delete()
        {
            Log.Debug("Entering Delete Method.");

            if (File.Exists(RepoFileName))
            {
                File.Delete(RepoFileName);
            }
            
            Log.Debug("Leaving Delete Method.");
        }

        protected string Read()
        {
            Log.Debug("Entering Read Method.");

            if (File.Exists(RepoFileName))
            {
                Log.Debug("Leaving Read Method.");
                return Crypt.Decrypt(File.ReadAllText(RepoFileName), CryptKey);
            }
            
            Log.Debug("Leaving Read Method.");
            return string.Empty;
        }

        protected void Write(string fileContent)
        {
            Log.Debug("In Write Method.");

            File.WriteAllText(RepoFileName, Crypt.Encrypt(fileContent, CryptKey));
        }
    }
}