using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty(PropertyName = "scope")]
        public string Scope { get; set; }

        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }

        public DateTime GetExpirationDate() => TokenCreationDate.AddSeconds(ExpiresIn);
    }
}