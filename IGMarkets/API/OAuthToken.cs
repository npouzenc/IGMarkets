using System;

namespace IGMarkets.API
{
    /// <summary>
    /// Pseudo-access token for consuming subsequent IG Trading API REST endpoints.
    /// </summary>
    public class OAuthToken
    {
        private DateTime TokenCreationDate;

        public OAuthToken()
        {
            TokenCreationDate = DateTime.Now;
        }
        public string Access_token { get; set; }

        public int Expires_in { get; set; }

        public string Refresh_token { get; set; }

        public string Scope { get; set; }

        public string Token_type { get; set; }

        public DateTime GetExpirationDate() => TokenCreationDate.AddSeconds(Expires_in);
    }
}