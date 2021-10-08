using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGMarkets.Core
{
    /// <summary>
    /// Creates a trading session and a streaming session for a valid IG account, obtaining session tokens for subsequent API accesses.
    /// </summary>
    public class Session
    {
        private ILogger<Session> logger;

        public Session(ILogger<Session> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Login to IGMarkets:
        /// - V2: uses CST TOken
        /// - V3: uses OAuthToken
        /// </summary>
        public void Login()
        {
            logger.LogInformation("Trying to connect to IG Markets.");
        }
    }
}
