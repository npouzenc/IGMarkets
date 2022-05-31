using Ardalis.GuardClauses;
using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGMarkets.API
{
    /// <summary>
    /// Base class for requesting IGMarkets' REST Trading API. Should be used only when a session has been already created (login passed).
    /// </summary>
    class IGRequest
    {
        /// <summary>
        /// Current session 
        /// </summary>
        public Session Session { get; private set; }

        /// <summary>
        /// API key to be used when calling IG Markets API
        /// </summary>
        internal Credentials Credentials { get; private set; }

        /// <summary>
        /// Base path of the IG Markets' REST Trading API
        /// </summary>
        public string Api { get; private set; } = "https://api.ig.com/gateway/deal";

        public IGRequest(Credentials credentials, Session session)
        {
            Guard.Against.Null(credentials, nameof(credentials));
            Guard.Against.Null(session, nameof(session));

            this.Session = session;
            this.Credentials = credentials;

            if (credentials.IsDemo)
            {
                Api = "https://demo-api.ig.com/gateway/deal";
            }
        }

        public IGRequest(Credentials credentials)
            : this(credentials, default(Session))
        {

        }

        public IFlurlRequest Endpoint(string path, int version = 1)
        {
            string endpoint = Url.Combine(Api, path);
            var request = endpoint.WithHeader("VERSION", version)
                    .WithHeader("X-IG-API-KEY", Credentials.ApiKey);
            if (Session.AccountId != null && Session.OAuthToken != null)
            {
                request
                    .WithHeader("IG-ACCOUNT-ID", Session.AccountId)
                    .WithOAuthBearerToken(Session.OAuthToken.Access_token);
            }
            
            return request;
        }
    }
}
