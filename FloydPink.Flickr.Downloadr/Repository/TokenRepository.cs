using System.IO;
using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Cryptography;

namespace FloydPink.Flickr.Downloadr.Repository
{
    public class TokenRepository : IRepository<Token>
    {
        private readonly string tokenFileName = "tokens.dat";
        private readonly string cryptKey = "SomeEncryPtionKey123";

        public Token Get()
        {
            if (File.Exists(tokenFileName))
            {
                string[] tokenAndSecret = File.ReadAllText(tokenFileName).Split(' ');
                return new Token(Crypt.Decrypt(tokenAndSecret[0], cryptKey), Crypt.Decrypt(tokenAndSecret[1], cryptKey));
            }
            return new Token(string.Empty, string.Empty);
        }

        public void Save(Token token)
        {
            File.WriteAllText(tokenFileName, string.Format("{0} {1}", Crypt.Encrypt(token.TokenString, cryptKey), Crypt.Encrypt(token.Secret, cryptKey)));
        }

        public void Delete()
        {
            if (File.Exists(tokenFileName))
            {
                File.Delete(tokenFileName);
            }
        }
    }
}
