using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IGMarkets.Core.Resources;

namespace IGMarkets.Core
{
    internal class Credentials
    {
        public string Identifier { get; }

        public string Password { get; }

        public string APIKey { get; }

        public OAuthToken OAuthToken { get; set; }

        public Credentials(string identifier, string password, string apiKey)
        {
            Guard.Against.NullOrWhiteSpace(identifier, nameof(identifier));
            Identifier = identifier;

            Guard.Against.NullOrWhiteSpace(password, nameof(password));
            Password = password;

            Guard.Against.NullOrWhiteSpace(apiKey, nameof(apiKey));
            APIKey = apiKey;
        }

        public override string ToString()
        {
            if (OAuthToken is null)
            {
                return $"Credentials for identifier {Identifier} with no defined OAuth token";
            }
            return $"Credentials for identifier {Identifier} - token expires in {OAuthToken.ExpiresIn} seconds.";
        }
    }
}
