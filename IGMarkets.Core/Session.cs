using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGMarkets.Core
{
    /// <summary>
    /// Session object build with IG authentication response.
    /// Cf. https://labs.ig.com/node/557
    /// Please note that V3 version of /session endpoint is not a so-called OAuth token.
    /// </summary>
    public class Session
    {
        public string AccountId { get; set; }

        public string ApplicationKey { get; set; }

        public string ClientId { get; set; }

        public string ClientSessionToken { get; set; }

        public string SecurityToken { get; set; }

        public Uri StreamingUrl { get; set; }
    }       
}
