using System;
using System.Collections.Generic;
using DotNetOpenAuth.OAuth.ChannelElements;
using DotNetOpenAuth.OAuth.Messages;
using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Repository;
using log4net;

namespace FloydPink.Flickr.Downloadr.OAuth
{
    public class TokenManager : IConsumerTokenManager
    {
        
        private static readonly ILog Log = LogManager.GetLogger(typeof(TokenManager));
        private readonly IRepository<Token> _tokenRepository;

        private readonly Dictionary<string, Tuple<string, TokenType>> _tokens =
            new Dictionary<string, Tuple<string, TokenType>>();

        public TokenManager(string consumerKey, string consumerSecret, IRepository<Token> tokenRepository)
        {
            if (String.IsNullOrEmpty(consumerKey))
            {
                throw new ArgumentNullException("consumerKey");
            }

            _tokenRepository = tokenRepository;

            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;

            GetStoredAccessToken();
        }

        #region IConsumerTokenManager Members

        public string ConsumerKey { get; private set; }

        public string ConsumerSecret { get; private set; }

        public void ExpireRequestTokenAndStoreNewAccessToken(string consumerKey, string requestToken, string accessToken,
                                                             string accessTokenSecret)
        {
            Log.Debug("Entering ExpireRequestTokenAndStoreNewAccessToken Method.");

            _tokens.Remove(requestToken);
            _tokens[accessToken] = new Tuple<string, TokenType>(accessTokenSecret, TokenType.AccessToken);
            _tokenRepository.Save(new Token(accessToken, accessTokenSecret));
            
            Log.Debug("Leaving ExpireRequestTokenAndStoreNewAccessToken Method.");
        }

        public string GetTokenSecret(string token)
        {
            Log.Debug("In GetTokenSecret Method.");

            return _tokens[token].Item1;
        }

        public TokenType GetTokenType(string token)
        {
            Log.Debug("In GetTokenType Method.");

            return _tokens[token].Item2;
        }

        public void StoreNewRequestToken(UnauthorizedTokenRequest request, ITokenSecretContainingMessage response)
        {
            Log.Debug("Entering StoreNewRequestToken Method.");

            _tokens[response.Token] = new Tuple<string, TokenType>(response.TokenSecret, TokenType.RequestToken);

            
            Log.Debug("Leaving StoreNewRequestToken Method.");
        }

        #endregion

        private void GetStoredAccessToken()
        {
            Log.Debug("Entering GetStoredAccessToken Method.");

            Token token = _tokenRepository.Get();
            if (!string.IsNullOrEmpty(token.TokenString))
            {
                _tokens[token.TokenString] = new Tuple<string, TokenType>(token.Secret, TokenType.AccessToken);
            }
            
            Log.Debug("Leaving GetStoredAccessToken Method.");
        }
    }
}