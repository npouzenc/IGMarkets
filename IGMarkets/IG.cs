using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGMarkets
{
    public static class IG
    {
        /// <summary>
        /// Entry point to create a new trading session on IGMarkets with specified credentials.
        /// </summary>
        /// <param name="identifier">Username of the account to connect to.</param>
        /// <param name="password">Password to use</param>
        /// <param name="apiKey">The API key k (obtained from My Account on our dealing platform) is how we identify and authorise the calling application</param>
        /// <param name="isDemo">Are you using a LIVE account or a DEMO account?</param>
        /// <returns>A new <see cref="ITrading"/> instance allowing a trading session for the credentials provided.</returns>
        public static Trading Connect(string identifier, string password, string apiKey, bool isDemo = false)
        {
            var tradingSession = new Trading();
            tradingSession.Login(identifier, password, apiKey, isDemo).Wait();
            return tradingSession;
        }
    }
}
