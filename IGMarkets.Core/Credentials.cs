using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IGMarkets.Core.Resources;

namespace IGMarkets.Core
{
    /// <summary>
    /// Credentials provided by the user/app.
    /// </summary>
    internal class Credentials
    {
        public string Identifier { get; }

        public string Password { get; }

        public string ApiKey { get; }

        public bool IsDemo { get; private set; }

        public Credentials(string identifier, string password, string apiKey, bool isDemo)
        {
            Guard.Against.NullOrWhiteSpace(identifier, nameof(identifier));
            Identifier = identifier;

            Guard.Against.NullOrWhiteSpace(password, nameof(password));
            Password = password;

            Guard.Against.NullOrWhiteSpace(apiKey, nameof(apiKey));
            ApiKey = apiKey;

            IsDemo = isDemo;
        }

        public override string ToString()
        {
            string message = $"Credentials for user {Identifier} and API key:{ApiKey.Substring(1, 5)}";
            return IsDemo ? message:message+" with DEMO account";
        }
    }
}
