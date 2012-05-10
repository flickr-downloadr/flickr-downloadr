using System.IO;
using FloydPink.Flickr.Downloadr.Cryptography;

namespace FloydPink.Flickr.Downloadr.Repository
{
    public abstract class RepositoryBase
    {
        internal abstract string repoFileName { get; }
        protected readonly string cryptKey = "SomeEncryPtionKey123";

        public void Delete()
        {
            if (File.Exists(repoFileName))
            {
                File.Delete(repoFileName);
            }
        }

        protected string Read()
        {
            if (File.Exists(repoFileName))
            {
                return Crypt.Decrypt(File.ReadAllText(repoFileName),cryptKey);
            }
            return string.Empty;
        }

        protected void Write(string fileContent)
        {
            File.WriteAllText(repoFileName, Crypt.Encrypt(fileContent, cryptKey));
        }

    }
}
