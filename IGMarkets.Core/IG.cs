using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGMarkets.Core
{
    public class IG : IDisposable
    {
        private Session session;
        private ILogger<IG> logger;

        public bool IsConnected { get; private set; }

        public IG(ILogger<IG> logger)
        {
            this.logger = logger;
            //session = new Session();
        }

        public void Login(string identifier, string password, string apiKey, bool demo = false)
        {
            logger.LogInformation("Trying to connect to IG Markets.");
            IsConnected = true;
        }

        public void Dispose()
        {
            // TODO: Force Disconnect
        }
    }
}
