using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGMarkets.API
{
    /// <summary>
    /// Session object build with IG authentication response.
    /// Cf. https://labs.ig.com/node/557
    /// </summary>
    [DebuggerDisplay("AccountId = {AccountId}, ClientId = {ClientId}")]
    public class Session
    {
        public string AccountId { get; set; }

        public string ClientId { get; set; }

        public int TimezoneOffset { get; set; }

        public OAuthToken OAuthToken { get; set; }

        public Uri LightstreamerEndpoint { get; set; }
    }       
}
